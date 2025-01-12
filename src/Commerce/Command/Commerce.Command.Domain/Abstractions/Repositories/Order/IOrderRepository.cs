using Entity = Commerce.Command.Domain.Entities.Order;

namespace Commerce.Command.Domain.Abstractions.Repositories.Order
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IOrderRepository : IGenericRepository<Entity.Order, Guid>
    {
    }
}