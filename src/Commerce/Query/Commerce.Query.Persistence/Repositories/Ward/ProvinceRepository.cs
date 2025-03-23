using Commerce.Query.Domain.Abstractions.Repositories.Ward;
using Entities = Commerce.Query.Domain.Entities.Ward;

namespace Commerce.Query.Persistence.Repositories.Ward
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class ProvinceRepository : GenericRepository<Entities.Province, int>, IProvinceRepository
    {

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public ProvinceRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}