using Commerce.Command.Domain.Abstractions.Repositories.Partner;
using Commerce.Command.Domain.Abstractions.Repositories.Settings;
using Entities = Commerce.Command.Domain.Entities.Partner;

namespace Commerce.Command.Persistence.Repositories.Partner
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class PartnerRepository : GenericRepository<Entities.Partner, Guid>, IPartnerRepository
    {
        private readonly ISignManager signManager;

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public PartnerRepository(ApplicationDbContext context, ISignManager signManager) : base(context)
        {
            this.signManager = signManager;
        }

        public virtual void Create(Entities.Partner entity)
        {
            entity.InsertedAt = DateTime.UtcNow;
            entity.InsertedBy = signManager.CurrentUser.Id;
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            Entities.Add(entity);
        }

        public virtual void Update(Entities.Partner entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = signManager.CurrentUser.Id;
            Entities.Update(entity);
        }
    }
}