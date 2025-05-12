using Commerce.Query.Domain.Abstractions.Repositories;
using Entity = Commerce.Command.Domain.Entities.Product;

namespace Commerce.Command.Domain.Abstractions.Repositories.Product
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IProductReviewReplyRepository : IGenericRepository<Entity.ProductReviewReply, Guid>
    {
    }
}