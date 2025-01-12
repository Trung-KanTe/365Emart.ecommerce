using Entity = Commerce.Command.Domain.Entities.ImportProduct;

namespace Commerce.Command.Domain.Abstractions.Repositories.ImportProduct
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IImportProductRepository : IGenericRepository<Entity.ImportProduct, Guid>
    {
    }
}