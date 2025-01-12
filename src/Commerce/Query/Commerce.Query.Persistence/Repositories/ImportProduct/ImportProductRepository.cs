using Commerce.Query.Domain.Abstractions.Repositories.ImportProduct;
using Commerce.Query.Domain.Abstractions.Repositories.Settings;
using Entities = Commerce.Query.Domain.Entities.ImportProduct;

namespace Commerce.Query.Persistence.Repositories.ImportProduct
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class ImportProductRepository : GenericRepository<Entities.ImportProduct, Guid>, IImportProductRepository
    {
        private readonly ISignManager signManager;

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public ImportProductRepository(ApplicationDbContext context, ISignManager signManager) : base(context)
        {
            this.signManager = signManager;
        }

        public virtual void Create(Entities.ImportProduct entity)
        {
            entity.InsertedAt = DateTime.UtcNow;
            entity.InsertedBy = signManager.CurrentUser.Id;
            entity.ImportDate = DateTime.UtcNow;
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            Entities.Add(entity);
        }

        public virtual void Update(Entities.ImportProduct entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = signManager.CurrentUser.Id;
            Entities.Update(entity);
        }
    }
}