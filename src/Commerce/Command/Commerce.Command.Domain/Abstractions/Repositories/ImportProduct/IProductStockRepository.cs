using Entity = Commerce.Command.Domain.Entities.ProductStock;

namespace Commerce.Command.Domain.Abstractions.Repositories.ProducStock
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IProductStockRepository : IGenericRepository<Entity.ProductStock, Guid>
    {
    }
}