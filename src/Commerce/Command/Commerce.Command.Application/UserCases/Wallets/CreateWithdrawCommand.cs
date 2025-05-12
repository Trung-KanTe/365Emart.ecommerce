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
    public record CreateWithdrawCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// Handler for create shopWallet request
    /// </summary>
    public class CreateWithdrawCommandHandler : IRequestHandler<CreateWithdrawCommand, Result>
    {
        private readonly IShopWalletRepository shopWalletRepository;
        private readonly IShopWalletTransactionRepository shopWalletTransactionRepository;
        private readonly IPlatformWalletRepository platformWalletRepository;
        private readonly IPlatformWalletTransactionRepository PlatformWalletTransactionRepository;
        private readonly IOrderRepository orderRepository;

        /// <summary>
        /// Handler for create shopWallet request
        /// </summary>
        public CreateWithdrawCommandHandler(IShopWalletRepository shopWalletRepository,
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
        public async Task<Result> Handle(CreateWithdrawCommand request, CancellationToken cancellationToken)
        {

            using var transaction = await shopWalletRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                var wallet = await shopWalletRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);

                // Trừ tiền
                wallet.Balance -= request.Amount;
                shopWalletRepository.Update(wallet);

                // Ghi lịch sử giao dịch
                var transactionRecord = new ShopWalletTransaction
                {
                    ShopWalletId = wallet.Id,
                    Amount = request.Amount,
                    Type = "Withdraw",
                };
                shopWalletTransactionRepository.Create(transactionRecord);

                await shopWalletRepository.SaveChangesAsync(cancellationToken);
                await shopWalletTransactionRepository.SaveChangesAsync(cancellationToken);

                transaction.Commit();
                return Result.Ok();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}