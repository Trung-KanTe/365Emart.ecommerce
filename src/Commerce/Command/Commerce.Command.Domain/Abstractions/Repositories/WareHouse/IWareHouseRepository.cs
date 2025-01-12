using Entity = Commerce.Command.Domain.Entities.WareHouse;

namespace Commerce.Command.Domain.Abstractions.Repositories.WareHouse
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IWareHouseRepository : IGenericRepository<Entity.WareHouse, Guid>
    {
    }
}