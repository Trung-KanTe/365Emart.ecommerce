using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Entities = Commerce.Command.Domain.Entities.Order;
using Commerce.Command.Domain.Abstractions.Repositories.Order;
using MediatR;

namespace Commerce.Command.Application.UserCases.Order
{
    /// <summary>
    /// Request to delete orderCancel, contain orderCancel id
    /// </summary>
    public record UpdateOrderCancelCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public Guid? OrderId { get; set; }
        public string? Reason { get; set; }
        public decimal? RefundAmount { get; set; }
        public bool IsRefunded { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }

    /// <summary>
    /// Handler for delete orderCancel request
    /// </summary>
    public class UpdateOrderCancelCommandHandler : IRequestHandler<UpdateOrderCancelCommand, Result>
    {
        private readonly IOrderCancelRepository orderCancelRepository;

        /// <summary>
        /// Handler for delete orderCancel request
        /// </summary>
        public UpdateOrderCancelCommandHandler(IOrderCancelRepository orderCancelRepository)
        {
            this.orderCancelRepository = orderCancelRepository;
        }

        /// <summary>
        /// Handle delete orderCancel request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateOrderCancelCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await orderCancelRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete orderCancel
                var orderCancel = await orderCancelRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (orderCancel == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.OrderCancel)) })));
                }
                // Update orderCancel, keep original data if request is null
                request.MapTo(orderCancel, true);
                orderCancel.ValidateUpdate();
                // Mark orderCancel as Updated state
                orderCancelRepository.Update(orderCancel);
                // Save orderCancel to database
                await orderCancelRepository.SaveChangesAsync(cancellationToken);
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