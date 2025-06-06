﻿using Commerce.Command.Contract.Contants;
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
    /// Request to delete orderCancel, contain orderCancel id
    /// </summary>
    public record DeleteOrderCancelCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete orderCancel, contain orderCancel id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete orderCancel request
    /// </summary>
    public class DeleteOrderCancelCommandHandler : IRequestHandler<DeleteOrderCancelCommand, Result>
    {
        private readonly IOrderCancelRepository orderCancelRepository;

        /// <summary>
        /// Handler for delete orderCancel request
        /// </summary>
        public DeleteOrderCancelCommandHandler(IOrderCancelRepository orderCancelRepository)
        {
            this.orderCancelRepository = orderCancelRepository;
        }

        /// <summary>
        /// Handle delete orderCancel request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(DeleteOrderCancelCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await orderCancelRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete orderCancel
                var orderCancel = await orderCancelRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (orderCancel == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.OrderCancel)) })));
                }
                orderCancel.IsDeleted = !orderCancel.IsDeleted;
                orderCancelRepository.Update(orderCancel);
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