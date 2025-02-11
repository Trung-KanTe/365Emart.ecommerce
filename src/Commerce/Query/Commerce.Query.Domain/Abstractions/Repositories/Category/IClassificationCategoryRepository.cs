using Entity = Commerce.Query.Domain.Entities.Category;

namespace Commerce.Query.Domain.Abstractions.Repositories.Category
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IClassificationCategoryRepository : IGenericRepository<Entity.ClassificationCategory, Guid>
    {
    }
}
