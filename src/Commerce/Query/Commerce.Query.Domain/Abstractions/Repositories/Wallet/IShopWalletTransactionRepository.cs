using Commerce.Query.Domain.Entities.Wallets;

namespace Commerce.Query.Domain.Abstractions.Repositories.Wallets
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IShopWalletTransactionRepository : IGenericRepository<ShopWalletTransaction, Guid>
    {
    }
}