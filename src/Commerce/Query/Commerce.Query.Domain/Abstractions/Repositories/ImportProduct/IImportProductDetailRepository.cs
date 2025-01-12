using Entity = Commerce.Query.Domain.Entities.ImportProduct;

namespace Commerce.Query.Domain.Abstractions.Repositories.ImportProduct
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IImportProductDetailRepository : IGenericRepository<Entity.ImportProductDetails, Guid>
    {
    }
}