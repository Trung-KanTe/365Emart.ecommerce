using Entity = Commerce.Command.Domain.Entities.Brand;

namespace Commerce.Command.Domain.Abstractions.Repositories.Brand
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IBrandRepository : IGenericRepository<Entity.Brand, Guid>
    {
    }
}