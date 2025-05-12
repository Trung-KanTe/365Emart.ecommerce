using Commerce.Query.Domain.Entities.Wallets;

namespace Commerce.Query.Domain.Abstractions.Repositories.Wallets
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IPlatformWalletRepository : IGenericRepository<PlatformWallet, Guid>
    {
    }
}