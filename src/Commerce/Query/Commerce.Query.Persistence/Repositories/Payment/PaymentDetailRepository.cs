using Commerce.Query.Domain.Abstractions.Repositories.Payment;
using Entities = Commerce.Query.Domain.Entities.Payment;

namespace Commerce.Query.Persistence.Repositories.Payment
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class PaymentDetailRepository : GenericRepository<Entities.PaymentDetails, Guid>, IPaymentDetailRepository
    {

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public PaymentDetailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}