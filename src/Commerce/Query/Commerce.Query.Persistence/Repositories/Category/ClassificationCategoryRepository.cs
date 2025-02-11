using Commerce.Query.Domain.Abstractions.Repositories.Category;
using Entities = Commerce.Query.Domain.Entities.Category;

namespace Commerce.Query.Persistence.Repositories.Category
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class ClassificationCategoryRepository : GenericRepository<Entities.ClassificationCategory, Guid>, IClassificationCategoryRepository
    {

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public ClassificationCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }      
    }
}