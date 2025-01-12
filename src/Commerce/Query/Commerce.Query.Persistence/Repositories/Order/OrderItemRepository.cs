using Commerce.Query.Domain.Abstractions.Repositories.Order;
using Commerce.Query.Domain.Abstractions.Repositories.Settings;
using Entities = Commerce.Query.Domain.Entities.Order;

namespace Commerce.Query.Persistence.Repositories.Order
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class OrderItemRepository : GenericRepository<Entities.OrderItem, Guid>, IOrderItemRepository
    {
        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public OrderItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}