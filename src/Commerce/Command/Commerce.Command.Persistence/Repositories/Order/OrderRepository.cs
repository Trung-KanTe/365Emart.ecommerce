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
            entity.Status = "đang chờ xử lý";
            entity.PaymentMethod = "Direct Check";
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            Entities.Add(entity);
        }

        public virtual void Update(Entities.Order entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = signManager.CurrentUser.Id;
            Entities.Update(entity);
        }
    }
}