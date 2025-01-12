using Commerce.Command.Domain.Abstractions.Repositories.Promotion;
using Commerce.Command.Domain.Abstractions.Repositories.Settings;
using Entities = Commerce.Command.Domain.Entities.Promotion;

namespace Commerce.Command.Persistence.Repositories.Promotion
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class PromotionRepository : GenericRepository<Entities.Promotion, Guid>, IPromotionRepository
    {
        private readonly ISignManager signManager;

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public PromotionRepository(ApplicationDbContext context, ISignManager signManager) : base(context)
        {
            this.signManager = signManager;
        }

        public virtual void Create(Entities.Promotion entity)
        {
            entity.InsertedAt = DateTime.UtcNow;
            entity.InsertedBy = signManager.CurrentUser.Id;
            entity.StartDate = DateTime.UtcNow;
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            Entities.Add(entity);
        }

        public virtual void Update(Entities.Promotion entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = signManager.CurrentUser.Id;
            Entities.Update(entity);
        }
    }
}