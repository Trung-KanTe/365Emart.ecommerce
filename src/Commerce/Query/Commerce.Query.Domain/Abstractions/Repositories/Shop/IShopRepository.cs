using Entity = Commerce.Query.Domain.Entities.Shop;

namespace Commerce.Query.Domain.Abstractions.Repositories.Shop
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IShopRepository : IGenericRepository<Entity.Shop, Guid>
    {
    }
}