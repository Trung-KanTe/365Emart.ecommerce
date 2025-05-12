using Commerce.Query.Application.DTOs.Shop;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using Commerce.Query.Domain.Abstractions.Repositories.Wallets;
using MediatR;

namespace Commerce.Query.Application.UserCases.Wallets
{
    /// <summary>
    /// Request to get brand by id
    /// </summary>
    public record GetAllWalletByShopQuery : IRequest<Result<WalletDTO>>
    {
        public Guid? Id { get; init; }

        public GetAllWalletByShopQuery() { }

        public GetAllWalletByShopQuery(Guid? id)
        {
            Id = id;
        }
    }

    /// Handle request
    /// </summary>
    /// <param name="request">Request to handle</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Result with brand data</returns>
    public class GetAllWalletByShopQueryHandler : IRequestHandler<GetAllWalletByShopQuery, Result<WalletDTO>>
    {
        private readonly IShopWalletRepository shopWalletRepository;
        private readonly IShopWalletTransactionRepository shopWalletTransactionRepository;
        private readonly IShopRepository shopRepository;

        public GetAllWalletByShopQueryHandler(
            IShopWalletRepository shopWalletRepository,
            IShopWalletTransactionRepository shopWalletTransactionRepository,
            IShopRepository shopRepository)
        {
            this.shopWalletRepository = shopWalletRepository;
            this.shopWalletTransactionRepository = shopWalletTransactionRepository;
            this.shopRepository = shopRepository;
        }

        public async Task<Result<WalletDTO>> Handle(GetAllWalletByShopQuery request, CancellationToken cancellationToken)
        {
            var shop = await shopRepository.FindSingleAsync(x => x.UserId == request.Id!.Value, true, cancellationToken);

            var wallet = await shopWalletRepository.FindSingleAsync(x => x.ShopId == shop.Id, true, cancellationToken);

            // Lấy tất cả transaction của wallet
            var transactions = shopWalletTransactionRepository.FindAll(x => x.ShopWalletId == wallet.Id);

            // Map DTO
            var dto = new WalletDTO
            {
                Id = wallet.Id,
                ShopId = wallet.ShopId,
                Balance = wallet.Balance,
                WalletTransactionDTOs = transactions.Select(t => new WalletTransactionDTO
                {
                    Id = t.Id,
                    ShopWalletId = t.ShopWalletId,
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