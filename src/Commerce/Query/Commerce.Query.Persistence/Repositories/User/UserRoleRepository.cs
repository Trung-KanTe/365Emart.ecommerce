using Commerce.Query.Domain.Abstractions.Repositories.User;
using Entities = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Persistence.Repositories.User
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class UserRoleRepository : GenericRepository<Entities.UserRole, Guid>, IUserRoleRepository
    {

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {
        }      
    }
}