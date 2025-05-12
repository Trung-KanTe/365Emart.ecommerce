using Commerce.Query.Domain.Abstractions.Repositories.Settings;
using Commerce.Query.Domain.Abstractions.Repositories.Wallets;
using Commerce.Query.Domain.Entities.Wallets;

namespace Commerce.Query.Persistence.Repositories
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class PlatformWalletRepository : GenericRepository<PlatformWallet, Guid>, IPlatformWalletRepository
    {
        private readonly ISignManager signManager;

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public PlatformWalletRepository(ApplicationDbContext context, ISignManager signManager) : base(context)
        {
            this.signManager = signManager;
        }

        public virtual void Create(PlatformWallet entity)
        {
            entity.InsertedAt = DateTime.UtcNow;
            entity.InsertedBy = signManager.CurrentUser.Id;
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            Entities.Add(entity);
        }

        public virtual void Update(PlatformWallet entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = signManager.CurrentUser.Id;
            Entities.Update(entity);
        }
    }
}