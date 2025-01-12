using Entity = Commerce.Command.Domain.Entities.Shop;

namespace Commerce.Command.Domain.Abstractions.Repositories.Shop
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IShopRepository : IGenericRepository<Entity.Shop, Guid>
    {
    }
}