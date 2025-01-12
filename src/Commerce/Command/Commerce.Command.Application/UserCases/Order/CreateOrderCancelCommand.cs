using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Order;
using Entities = Commerce.Command.Domain.Entities.Order;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;

namespace Commerce.Command.Application.UserCases.Order
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateOrderCancelCommand : IRequest<Result<Entities.OrderCancel>>
    {
        public Guid? OrderId { get; set; }
        public string? Reason { get; set; }
        public decimal? RefundAmount { get; set; }
        public bool IsRefunded { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }

    /// <summary>
    /// Handler for create orderCancel request
    /// </summary>
    public class CreateOrderCancelCommandHandler : IRequestHandler<CreateOrderCancelCommand, Result<Entities.OrderCancel>>
    {
        private readonly IOrderCancelRepository orderCancelRepository;

        /// <summary>
        /// Handler for create orderCancel request
        /// </summary>
        public CreateOrderCancelCommandHandler(IOrderCancelRepository orderCancelRepository)
        {
            this.orderCancelRepository = orderCancelRepository;
        }

        /// <summary>
        /// Handle create orderCancel request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with orderCancel data</returns>
        public async Task<Result<Entities.OrderCancel>> Handle(CreateOrderCancelCommand request, CancellationToken cancellationToken)
        {
            // Create new OrderCancel from request
            Entities.OrderCancel? orderCancel = request.MapTo<Entities.OrderCancel>();
            // Validate for orderCancel
            orderCancel!.ValidateCreate();
            // Begin transaction
            using var transaction = await orderCancelRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Add data
                orderCancelRepository.Create(orderCancel!);
                // Save data
                await orderCancelRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return orderCancel;
            }
            catch (Exception)
            {
                // Rollback transaction
                transaction.Rollback();
                throw;
            }
        }
    }
}