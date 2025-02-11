using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Persistence.Repositories.Product
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class ProductDetailRepository : GenericRepository<Entities.ProductDetail, Guid>, IProductDetailRepository
    {

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public ProductDetailRepository(ApplicationDbContext context) : base(context)
        {
        }     
    }
}