using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Microsoft.EntityFrameworkCore;
using Entities = Commerce.Command.Domain.Entities.Product;

namespace Commerce.Command.Persistence.Repositories.Product
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

        public async Task RemoveMulti(Guid productId)
        {          
        }
    }
}