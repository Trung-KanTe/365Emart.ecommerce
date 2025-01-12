using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.Order;
using Entities = Commerce.Command.Domain.Entities.Order;
using MediatR;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;

namespace Commerce.Command.Application.UserCases.Order
{
    /// <summary>
    /// Request to delete order, contain order id
    /// </summary>
    public record DeleteOrderCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete order, contain order id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete order request
    /// </summary>
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteOrderCommand, Result>
    {
        private readonly IOrderRepository orderRepository;

        /// <summary>
        /// Handler for delete order request
        /// </summary>
        public DeleteCategoryCommandHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        /// <summary>
        /// Handle delete order request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await orderRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete order
                var order = await orderRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (order == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Order)) })));
                }
                order.IsDeleted = false;
                orderRepository.Update(order);
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