using Commerce.Query.Domain.Abstractions.Repositories.User;
using Entities = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Persistence.Repositories.User
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