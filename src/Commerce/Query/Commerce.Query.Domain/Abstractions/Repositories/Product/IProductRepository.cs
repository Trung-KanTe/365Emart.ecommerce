using Entity = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Domain.Abstractions.Repositories.Product
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IProductRepository : IGenericRepository<Entity.Product, Guid>
    {
    }
}