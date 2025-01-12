using Entity = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Domain.Abstractions.Repositories.User
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IRoleRepository : IGenericRepository<Entity.Role, Guid>
    {
    }
}