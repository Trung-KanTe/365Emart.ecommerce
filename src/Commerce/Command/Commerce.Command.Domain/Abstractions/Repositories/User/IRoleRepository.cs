using Entity = Commerce.Command.Domain.Entities.User;

namespace Commerce.Command.Domain.Abstractions.Repositories.User
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IRoleRepository : IGenericRepository<Entity.Role, Guid>
    {
    }
}