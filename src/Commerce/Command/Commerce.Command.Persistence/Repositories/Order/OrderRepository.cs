using Commerce.Command.Domain.Abstractions.Repositories.Order;
using Commerce.Command.Domain.Abstractions.Repositories.Settings;
using Entities = Commerce.Command.Domain.Entities.Order;

namespace Commerce.Command.Persistence.Repositories.Order
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class OrderRepository : GenericRepository<Entities.Order, Guid>, IOrderRepository
    {
        private readonly ISignManager signManager;

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public OrderRepository(ApplicationDbContext context, ISignManager signManager) : base(context)
        {
            this.signManager = signManager;
        }

        public virtual void Create(Entities.Order entity)
        {
            entity.InsertedAt = DateTime.UtcNow;
            entity.InsertedBy = signManager.CurrentUser.Id;
            entity.Status = "pending";
            entity.PaymentMethod = "Cash";
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            Entities.Add(entity);
        }
    }
}