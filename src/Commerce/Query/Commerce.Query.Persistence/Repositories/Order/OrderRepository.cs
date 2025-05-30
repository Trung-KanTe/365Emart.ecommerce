﻿using Commerce.Query.Domain.Abstractions.Repositories.Order;
using Commerce.Query.Domain.Abstractions.Repositories.Settings;
using Entities = Commerce.Query.Domain.Entities.Order;

namespace Commerce.Query.Persistence.Repositories.Order
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
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            Entities.Add(entity);
        }

        public virtual void Update(Entities.Order entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            Entities.Update(entity);
        }
    }
}