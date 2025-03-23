using Entity = Commerce.Query.Domain.Entities.Ward;

namespace Commerce.Query.Domain.Abstractions.Repositories.Ward
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IDistrictRepository : IGenericRepository<Entity.District, int>
    {
    }
}