using Commerce.Query.Domain.Abstractions.Repositories.Wallets;
using Commerce.Query.Domain.Entities.Wallets;

namespace Commerce.Query.Persistence.Repositories
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class ShopWalletTransactionRepository : GenericRepository<ShopWalletTransaction, Guid>, IShopWalletTransactionRepository
    {

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public ShopWalletTransactionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public virtual void Create(ShopWalletTransaction entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            Entities.Add(entity);
        }
    }
}