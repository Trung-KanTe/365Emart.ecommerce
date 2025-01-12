using Entity = Commerce.Command.Domain.Entities.Product;

namespace Commerce.Command.Domain.Abstractions.Repositories.Product
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IProductReviewRepository : IGenericRepository<Entity.ProductReview, Guid>
    {
    }
}