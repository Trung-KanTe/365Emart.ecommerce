using Entity = Commerce.Command.Domain.Entities.Promotion;

namespace Commerce.Command.Domain.Abstractions.Repositories.Promotion
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IPromotionRepository : IGenericRepository<Entity.Promotion, Guid>
    {
    }
}