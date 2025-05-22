using Commerce.Command.Contract.Abstractions;
using Entities = Commerce.Command.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Command.Persistence.Repositories
{
    public class OrderService : IOrderService
    {
        protected readonly ApplicationDbContext context;
        public DbSet<Entities.Order> Orders { get; set; }
        public DbSet<Entities.OrderCancel> OrderCancels { get; set; }

        public OrderService(ApplicationDbContext context)
        {
            this.context = context;
        }

        protected DbSet<Entities.Order> Order
        {
            get
            {
                if (Orders == null) Orders = context.Set<Entities.Order>();
                return Orders;
            }
        }

        protected DbSet<Entities.OrderCancel> OrderCancel
        {
            get
            {
                if (OrderCancels == null) OrderCancels = context.Set<Entities.OrderCancel>();
                return OrderCancels;
            }
        }

        public async Task CancelPendingOrdersAsync()
        {
            var twoDaysAgo = DateTime.UtcNow.AddDays(-2);

            var pendingOrders = await context.Orders
                .Where(o => o.Status == "pending" && o.InsertedAt < twoDaysAgo)
                .ToListAsync();

            foreach (var order in pendingOrders)
            {
                order.Status = "canceled";
                order.UpdatedAt = DateTime.UtcNow;
                order.UpdatedBy = Guid.Empty; 

                var cancellation = new Entities.OrderCancel
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    Reason = "Do not approve orders",
                    RefundAmount = 0,
                    IsRefunded = false,
                    InsertedAt = DateTime.UtcNow,
                    InsertedBy = Guid.Empty,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = Guid.Empty,
                    IsDeleted = false
                };

                context.OrderCancels.Add(cancellation);
            }

            await context.SaveChangesAsync();
        }
    }
}