using Entity = Commerce.Command.Domain.Entities.Payment;

namespace Commerce.Command.Domain.Abstractions.Repositories.Payment
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IPaymentRepository : IGenericRepository<Entity.Payment, Guid>
    {
    }
}