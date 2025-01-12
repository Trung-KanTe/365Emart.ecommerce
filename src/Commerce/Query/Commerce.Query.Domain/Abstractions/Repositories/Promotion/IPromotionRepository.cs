using Entity = Commerce.Query.Domain.Entities.Promotion;

namespace Commerce.Query.Domain.Abstractions.Repositories.Promotion
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IPromotionRepository : IGenericRepository<Entity.Promotion, Guid>
    {
    }
}