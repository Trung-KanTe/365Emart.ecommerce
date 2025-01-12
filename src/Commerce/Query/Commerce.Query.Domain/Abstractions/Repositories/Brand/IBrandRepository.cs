using Entity = Commerce.Query.Domain.Entities.Brand;

namespace Commerce.Query.Domain.Abstractions.Repositories.Brand
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IBrandRepository : IGenericRepository<Entity.Brand, Guid>
    {
    }
}