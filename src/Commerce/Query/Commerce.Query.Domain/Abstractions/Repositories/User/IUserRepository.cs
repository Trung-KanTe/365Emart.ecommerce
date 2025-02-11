using Commerce.Query.Contract.Shared;
using Entity = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Domain.Abstractions.Repositories.User
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IUserRepository : IGenericRepository<Entity.User, Guid>
    {      
    }
}