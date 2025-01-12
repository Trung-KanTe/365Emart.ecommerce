using Entity = Commerce.Command.Domain.Entities.Category;

namespace Commerce.Command.Domain.Abstractions.Repositories.Category
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface ICategoryRepository : IGenericRepository<Entity.Category, Guid>
    {
    }
}