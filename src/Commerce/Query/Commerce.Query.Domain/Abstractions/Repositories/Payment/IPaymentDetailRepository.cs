using Entity = Commerce.Query.Domain.Entities.Payment;

namespace Commerce.Query.Domain.Abstractions.Repositories.Payment
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IPaymentDetailRepository : IGenericRepository<Entity.PaymentDetails, Guid>
    {
    }
}