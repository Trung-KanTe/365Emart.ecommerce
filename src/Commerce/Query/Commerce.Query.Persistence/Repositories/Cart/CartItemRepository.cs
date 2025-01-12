using Commerce.Query.Domain.Abstractions.Repositories.Cart;
using Commerce.Query.Domain.Abstractions.Repositories.Settings;
using Entities = Commerce.Query.Domain.Entities.Cart;

namespace Commerce.Query.Persistence.Repositories.Cart
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class CartItemRepository : GenericRepository<Entities.CartItem, Guid>, ICartItemRepository
    {

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
        }   
    }
}