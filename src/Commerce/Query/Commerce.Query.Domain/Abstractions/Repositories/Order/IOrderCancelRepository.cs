using Entity = Commerce.Query.Domain.Entities.Order;

namespace Commerce.Query.Domain.Abstractions.Repositories.Order
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IOrderCancelRepository : IGenericRepository<Entity.OrderCancel, Guid>
    {
    }
}
