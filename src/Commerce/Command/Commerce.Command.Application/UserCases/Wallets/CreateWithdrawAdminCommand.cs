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
    public record CreateWithdrawAdminCommand : IRequest<Result>
    {
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// Handler for create shopWallet request
    /// </summary>
    public class CreateWithdrawAdminCommandHandler : IRequestHandler<CreateWithdrawAdminCommand, Result>
    {
        private readonly IShopWalletRepository shopWalletRepository;
        private readonly IShopWalletTransactionRepository shopWalletTransactionRepository;
        private readonly IPlatformWalletRepository platformWalletRepository;
        private readonly IPlatformWalletTransactionRepository PlatformWalletTransactionRepository;
        private readonly IOrderRepository orderRepository;

        /// <summary>
        /// Handler for create shopWallet request
        /// </summary>
        public CreateWithdrawAdminCommandHandler(IShopWalletRepository shopWalletRepository,
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
        public async Task<Result> Handle(CreateWithdrawAdminCommand request, CancellationToken cancellationToken)
        {

            using var transaction = await platformWalletRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                var wallet = platformWalletRepository.FindAll().FirstOrDefault();

                // Trừ tiền
                wallet.Balance -= request.Amount;
                platformWalletRepository.Update(wallet);

                // Ghi lịch sử giao dịch
                var transactionRecord = new PlatformWalletTransaction
                {
                    Amount = request.Amount,
                    Type = "Withdraw",
                };
                PlatformWalletTransactionRepository.Create(transactionRecord);

                await platformWalletRepository.SaveChangesAsync(cancellationToken);
                await PlatformWalletTransactionRepository.SaveChangesAsync(cancellationToken);

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