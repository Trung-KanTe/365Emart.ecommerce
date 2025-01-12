using Entity = Commerce.Query.Domain.Entities.WareHouse;

namespace Commerce.Query.Domain.Abstractions.Repositories.WareHouse
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IWareHouseRepository : IGenericRepository<Entity.WareHouse, Guid>
    {
    }
}