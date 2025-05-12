using Commerce.Command.Domain.Entities.Wallets;

namespace Commerce.Command.Domain.Abstractions.Repositories.Wallets
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IPlatformWalletRepository : IGenericRepository<PlatformWallet, Guid>
    {
    }
}