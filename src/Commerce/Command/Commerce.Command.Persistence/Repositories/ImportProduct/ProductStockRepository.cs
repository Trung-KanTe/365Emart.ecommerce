using Commerce.Command.Domain.Abstractions.Repositories.ImportProduct;
using Commerce.Command.Domain.Abstractions.Repositories.ProducStock;
using Commerce.Command.Domain.Abstractions.Repositories.Settings;
using Entities = Commerce.Command.Domain.Entities.ProductStock;

namespace Commerce.Command.Persistence.Repositories.ImportProduct
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class ProductStockRepository : GenericRepository<Entities.ProductStock, Guid>, IProductStockRepository
    {    
        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public ProductStockRepository(ApplicationDbContext context) : base(context)
        {
        }      
    }
}
