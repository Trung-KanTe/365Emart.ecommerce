using Commerce.Command.Domain.Abstractions.Repositories.Payment;
using Commerce.Command.Domain.Abstractions.Repositories.Settings;
using Microsoft.EntityFrameworkCore;
using Entities = Commerce.Command.Domain.Entities.Payment;

namespace Commerce.Command.Persistence.Repositories.Payment
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class PaymentRepository : GenericRepository<Entities.Payment, Guid>, IPaymentRepository
    {
        private readonly ISignManager signManager;

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public PaymentRepository(ApplicationDbContext context, ISignManager signManager) : base(context)
        {
            this.signManager = signManager;
        }

        public virtual void Create(Entities.Payment entity)
        {
            entity.InsertedAt = DateTime.UtcNow;
            //entity.InsertedBy = signManager.CurrentUser.Id;
            entity.PaymentDate = DateTime.UtcNow;          
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            Entities.Add(entity);
        }

        public virtual void Update(Entities.Payment entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            //entity.UpdatedBy = signManager.CurrentUser.Id;
            Entities.Update(entity);
        }

        public async Task<Entities.Payment> GetByBankCodeAsync(long bankCode)
        {
            var payment = await Entities.AsNoTracking().FirstOrDefaultAsync(p => p.BankCode == bankCode);
            return payment;
        }

    }
}