using Commerce.Command.Contract.Abstractions;
using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.Order;
using Commerce.Command.Domain.Abstractions.Repositories.Payment;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using Commerce.Command.Domain.Abstractions.Repositories.Wallets;
using Commerce.Command.Domain.Entities.Wallets;
using Hangfire;
using MediatR;
using Entities = Commerce.Command.Domain.Entities.Order;

namespace Commerce.Command.Application.UserCases.Order
{
    /// <summary>
    /// Request to delete order, contain order id
    /// </summary>
    public record UpdateStatusOrderCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete order request
    /// </summary>
    public class UpdateStatusOrderCommandHandler : IRequestHandler<UpdateStatusOrderCommand, Result>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IPaymentRepository paymentRepository;
        private readonly IUserRepository userRepository;
        private readonly IEmailSender emailSender;
        private readonly IShopWalletRepository shopWalletRepository;
        private readonly IShopWalletTransactionRepository shopWalletTransactionRepository;
        private readonly IPlatformWalletRepository platformWalletRepository;
        private readonly IPlatformWalletTransactionRepository PlatformWalletTransactionRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly IProductRepository productRepository;

        /// <summary>
        /// Handler for create Order request
        /// </summary>
        public UpdateStatusOrderCommandHandler(IOrderRepository orderRepository, IPaymentRepository paymentRepository, IUserRepository userRepository, IEmailSender emailSender,
                                                IShopWalletRepository shopWalletRepository,
                                                IShopWalletTransactionRepository shopWalletTransactionRepository,
                                                IPlatformWalletRepository platformWalletRepository,
                                                IPlatformWalletTransactionRepository PlatformWalletTransactionRepository,
                                                IProductDetailRepository productDetailRepository,
                                                IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.paymentRepository = paymentRepository;
            this.userRepository = userRepository;
            this.emailSender = emailSender;
            this.shopWalletRepository = shopWalletRepository;
            this.platformWalletRepository = platformWalletRepository;
            this.PlatformWalletTransactionRepository = PlatformWalletTransactionRepository;
            this.shopWalletTransactionRepository = shopWalletTransactionRepository;
            this.productDetailRepository = productDetailRepository;
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Handle delete order request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateStatusOrderCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await orderRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete order
                var order = await orderRepository.FindByIdAsync(request.Id.Value, true, cancellationToken, includeProperties: x => x.OrderItems!);
                if (order == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Order)) })));
                }

                var payment = await paymentRepository.FindSingleAsync(x => x.OrderId == request.Id);
                payment.Amount = order.TotalAmount;
 
                order.PaymentMethod = "VN-Pay";


                var commissionAmount = order!.TotalAmount!.Value * 0.1m;
                var shopNetAmount = order.TotalAmount - commissionAmount;

                var firstOrderItem = order.OrderItems!.FirstOrDefault();
                var productDetail = await productDetailRepository.FindByIdAsync(firstOrderItem!.ProductDetailId!.Value, true, cancellationToken);
                var product = await productRepository.FindByIdAsync(productDetail.ProductId.Value, true, cancellationToken);

                // 1. Kiểm tra ví Shop
                var shopWallet = await shopWalletRepository.FindSingleAsync(x => x.ShopId == product.ShopId, true, cancellationToken);
                if (shopWallet == null)
                {
                    shopWallet = new ShopWallet
                    {
                        ShopId = product.ShopId!.Value,
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
                    Type = "Income",
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


                // Mark order as Updated state
                orderRepository.Update(order);
                paymentRepository.Update(payment);
                // Save order to database
                await orderRepository.SaveChangesAsync(cancellationToken);
                await paymentRepository.SaveChangesAsync(cancellationToken);

                // Commit transaction
                transaction.Commit();
                return Result.Ok();
            }
            catch (Exception)
            {
                // Rollback transaction
                transaction.Rollback();
                throw;
            }
        }
    }
}