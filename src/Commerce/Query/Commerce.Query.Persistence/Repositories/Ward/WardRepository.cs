using Commerce.Query.Domain.Abstractions.Repositories.Ward;
using Entities = Commerce.Query.Domain.Entities.Ward;

namespace Commerce.Query.Persistence.Repositories.Ward
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class WardRepository : GenericRepository<Entities.Ward, int>, IWardRepository
    {

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public WardRepository(ApplicationDbContext context) : base(context)
        {
        }     
    }
}