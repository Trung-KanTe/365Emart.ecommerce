using Commerce.Command.Domain.Abstractions.Repositories.Wallets;
using Commerce.Command.Domain.Entities.Wallets;

namespace Commerce.Command.Persistence.Repositories
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