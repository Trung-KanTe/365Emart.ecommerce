using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.Order;
using Commerce.Command.Domain.Abstractions.Repositories.Promotion;
using Commerce.Command.Domain.Entities.Order;
using MediatR;
using Entiti = Commerce.Command.Domain.Entities.Promotion;
using Entities = Commerce.Command.Domain.Entities.Order;

namespace Commerce.Command.Application.UserCases.Order
{
    /// <summary>
    /// Request to delete order, contain order id
    /// </summary>
    public record UpdateOrderCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PromotionId { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public ICollection<OrderItem>? OrderItems { get; set; }
    }

    /// <summary>
    /// Handler for delete order request
    /// </summary>
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IPromotionRepository promotionRepository;

        /// <summary>
        /// Handler for create Order request
        /// </summary>
        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IPromotionRepository promotionRepository)
        {
            this.orderRepository = orderRepository;
            this.promotionRepository = promotionRepository;
        }

        /// <summary>
        /// Handle delete order request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await orderRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete order
                var order = await orderRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (order == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Order)) })));
                }
                // Update order, keep original data if request is null
                request.MapTo(order, true);               

                // Mark order as Updated state
                orderRepository.Update(order);
                // Save order to database
                await orderRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return Result.Ok();
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