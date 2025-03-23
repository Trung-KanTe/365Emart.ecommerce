using Commerce.Query.Domain.Abstractions.Repositories.Ward;
using Entities = Commerce.Query.Domain.Entities.Ward;

namespace Commerce.Query.Persistence.Repositories.Ward
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class DistrictRepository : GenericRepository<Entities.District, int>, IDistrictRepository
    {

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public DistrictRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}