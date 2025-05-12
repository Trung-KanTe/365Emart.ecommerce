using Commerce.Query.Domain.Abstractions.Repositories.Wallets;
using Commerce.Query.Domain.Entities.Wallets;

namespace Commerce.Query.Persistence.Repositories
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class PlatformWalletTransactionRepository : GenericRepository<PlatformWalletTransaction, Guid>, IPlatformWalletTransactionRepository
    {

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public PlatformWalletTransactionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public virtual void Create(PlatformWalletTransaction entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            Entities.Add(entity);
        }
    }
}