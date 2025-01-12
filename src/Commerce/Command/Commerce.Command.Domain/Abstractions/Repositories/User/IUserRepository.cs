using Entity = Commerce.Command.Domain.Entities.User;

namespace Commerce.Command.Domain.Abstractions.Repositories.User
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IUserRepository : IGenericRepository<Entity.User, Guid>
    {
    }
}