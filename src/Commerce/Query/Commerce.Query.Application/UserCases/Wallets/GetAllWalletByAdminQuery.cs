using Commerce.Query.Application.DTOs.Shop;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Wallets;
using MediatR;

namespace Commerce.Query.Application.UserCases.Wallets
{
    /// <summary>
    /// Request to get brand by id
    /// </summary>
    public record GetAllWalletByAdminQuery : IRequest<Result<WalletDTO>>
    {
        public GetAllWalletByAdminQuery() { }

    }

    /// Handle request
    /// </summary>
    /// <param name="request">Request to handle</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Result with brand data</returns>
    public class GetAllWalletByAdminQueryHandler : IRequestHandler<GetAllWalletByAdminQuery, Result<WalletDTO>>
    {
        private readonly IPlatformWalletRepository platformWalletRepository;
        private readonly IPlatformWalletTransactionRepository platformWalletTransactionRepository;

        public GetAllWalletByAdminQueryHandler(
            IPlatformWalletRepository platformWalletRepository,
            IPlatformWalletTransactionRepository platformWalletTransactionRepository)
        {
            this.platformWalletRepository = platformWalletRepository;
            this.platformWalletTransactionRepository = platformWalletTransactionRepository;
        }

        public async Task<Result<WalletDTO>> Handle(GetAllWalletByAdminQuery request, CancellationToken cancellationToken)
        {
            var wallet = platformWalletRepository.FindAll().FirstOrDefault();

            // Lấy tất cả transaction của wallet
            var transactions = platformWalletTransactionRepository.FindAll();

            // Map DTO
            var dto = new WalletDTO
            {
                Id = wallet.Id,
                Balance = wallet.Balance,
                WalletTransactionDTOs = transactions.Select(t => new WalletTransactionDTO
                {
                    Id = t.Id,
                    OrderId = t.OrderId,
                    Amount = t.Amount,
                    Type = t.Type,
                    CreatedAt = t.CreatedAt
                }).ToList()
            };

            return dto;

        }
    }
}