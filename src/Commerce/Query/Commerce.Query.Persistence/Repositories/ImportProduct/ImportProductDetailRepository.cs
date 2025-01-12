using Commerce.Query.Domain.Abstractions.Repositories.ImportProduct;
using Commerce.Query.Domain.Abstractions.Repositories.Settings;
using Entities = Commerce.Query.Domain.Entities.ImportProduct;

namespace Commerce.Query.Persistence.Repositories.ImportProduct
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class ImportProductDetailRepository : GenericRepository<Entities.ImportProductDetails, Guid>, IImportProductDetailRepository
    {
        private readonly ISignManager signManager;

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public ImportProductDetailRepository(ApplicationDbContext context) : base(context)
        {
        }     
    }
}