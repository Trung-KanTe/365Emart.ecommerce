using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Order;
using Commerce.Command.Domain.Abstractions.Repositories.Wallets;
using Commerce.Command.Domain.Entities.Wallets;
using MediatR;
using Entities = Commerce.Command.Domain.Entities.Order;

namespace Commerce.Command.Application.UserCases.Wallets
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateShopWalletCommand : IRequest<Result<ShopWallet>>
    {
        public Guid? OrderId { get; set; }
        public Guid? ShopId { get; set;}
        public string Type { get; set; }
    }

    /// <summary>
    /// Handler for create shopWallet request
    /// </summary>
    public class CreateShopWalletCommandHandler : IRequestHandler<CreateShopWalletCommand, Result<ShopWallet>>
    {
        private readonly IShopWalletRepository shopWalletRepository;
        private readonly IShopWalletTransactionRepository shopWalletTransactionRepository;
        private readonly IPlatformWalletRepository platformWalletRepository;
        private readonly IPlatformWalletTransactionRepository PlatformWalletTransactionRepository;
        private readonly IOrderRepository orderRepository;

        /// <summary>
        /// Handler for create shopWallet request
        /// </summary>
        public CreateShopWalletCommandHandler(IShopWalletRepository shopWalletRepository,
            IShopWalletTransactionRepository shopWalletTransactionRepository,
            IPlatformWalletRepository platformWalletRepository,
            IPlatformWalletTransactionRepository PlatformWalletTransactionRepository,
            IOrderRepository orderRepository)
        {
            this.shopWalletRepository = shopWalletRepository;
            this.platformWalletRepository = platformWalletRepository;
            this.PlatformWalletTransactionRepository = PlatformWalletTransactionRepository;
            this.shopWalletTransactionRepository = shopWalletTransactionRepository;
            this.orderRepository = orderRepository;
        }

        /// <summary>
        /// Handle create shopWallet request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with shopWallet data</returns>
        public async Task<Result<ShopWallet>> Handle(CreateShopWalletCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.OrderId!.Value, true, cancellationToken);

            if (order.PaymentMethod == "VN-Pay")
            {
                return Result.Ok();
            }

            var commissionAmount = order!.TotalAmount!.Value * 0.1m;
            var shopNetAmount = order.TotalAmount - commissionAmount;

            using var transaction = await shopWalletRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // 1. Kiểm tra ví Shop
                var shopWallet = await shopWalletRepository.FindSingleAsync(x => x.ShopId == request.ShopId, true, cancellationToken);
                if (shopWallet == null)
                {
                    shopWallet = new ShopWallet
                    {
                        ShopId = request.ShopId!.Value,
                        Balance = shopNetAmount.Value,
                    };
                    shopWalletRepository.Create(shopWallet);
                }
                else
                {
                    shopWallet.Balance += shopNetAmount.Value;
                    shopWalletRepository.Update(shopWallet);
                }

                // 2. Ghi lịch sử giao dịch của Shop
                var shopTransaction = new ShopWalletTransaction
                {
                    ShopWalletId = shopWallet.Id,
                    OrderId = order.Id,
                    Amount = shopNetAmount.Value,
                    Type = request.Type,
                };
                shopWalletTransactionRepository.Create(shopTransaction);

                // 3. Ghi nhận tiền hoa hồng cho Platform
                var platformWallet = platformWalletRepository.FindAll().FirstOrDefault();
                if (platformWallet == null)
                {
                    platformWallet = new PlatformWallet
                    {
                        Balance = commissionAmount
                    };
                    platformWalletRepository.Create(platformWallet);
                }
                else
                {
                    platformWallet.Balance += commissionAmount;
                    platformWalletRepository.Update(platformWallet);
                }

                // 4. Ghi lịch sử giao dịch Platform
                var platformTransaction = new PlatformWalletTransaction
                {
                    OrderId = order.Id,
                    Amount = commissionAmount,
                    Type = "Commission",
                };
                PlatformWalletTransactionRepository.Create(platformTransaction);

                // Lưu thay đổi
                await shopWalletRepository.SaveChangesAsync(cancellationToken);
                await platformWalletRepository.SaveChangesAsync(cancellationToken);
                await shopWalletTransactionRepository.SaveChangesAsync(cancellationToken);
                await PlatformWalletTransactionRepository.SaveChangesAsync(cancellationToken);

                transaction.Commit();
                return shopWallet;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}