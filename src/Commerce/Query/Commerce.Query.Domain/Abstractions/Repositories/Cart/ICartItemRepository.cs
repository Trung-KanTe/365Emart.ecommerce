using Entity = Commerce.Query.Domain.Entities.Cart;

namespace Commerce.Query.Domain.Abstractions.Repositories.Cart
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface ICartItemRepository : IGenericRepository<Entity.CartItem, Guid>
    {
    }
}