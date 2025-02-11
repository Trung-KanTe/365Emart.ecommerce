using Entity = Commerce.Command.Domain.Entities.Cart;

namespace Commerce.Command.Domain.Abstractions.Repositories.Cart
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface ICartItemRepository : IGenericRepository<Entity.CartItem, Guid>
    {
    }
}