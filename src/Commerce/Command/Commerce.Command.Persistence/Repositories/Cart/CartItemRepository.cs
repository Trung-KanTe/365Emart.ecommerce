using Commerce.Command.Domain.Abstractions.Repositories.Cart;
using Entities = Commerce.Command.Domain.Entities.Cart;

namespace Commerce.Command.Persistence.Repositories.Cart
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
