using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.Settings;
using Commerce.Query.Persistence;
using Commerce.Query.Persistence.Repositories;
using Entities = Commerce.Command.Domain.Entities.Product;

namespace Commerce.Command.Persistence.Repositories.Product
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class ProductReviewReplyRepository : GenericRepository<Entities.ProductReviewReply, Guid>, IProductReviewReplyRepository
    {
        private readonly ISignManager signManager;

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public ProductReviewReplyRepository(ApplicationDbContext context, ISignManager signManager) : base(context)
        {
            this.signManager = signManager;
        }

        public virtual void Create(Entities.ProductReviewReply entity)
        {
            entity.InsertedAt = DateTime.UtcNow;
            entity.InsertedBy = signManager.CurrentUser.Id;
            entity.IsDeleted = true;
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            Entities.Add(entity);
        }

        public virtual void Update(Entities.ProductReviewReply entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = signManager.CurrentUser.Id;
            Entities.Update(entity);
        }
    }
}