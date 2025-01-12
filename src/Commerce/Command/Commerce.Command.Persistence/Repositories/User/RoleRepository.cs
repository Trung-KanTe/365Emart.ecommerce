using Commerce.Command.Domain.Abstractions.Repositories.User;
using Entities = Commerce.Command.Domain.Entities.User;

namespace Commerce.Command.Persistence.Repositories.User
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class RoleRepository : GenericRepository<Entities.Role, Guid>, IRoleRepository
    {
        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }       
    }
}