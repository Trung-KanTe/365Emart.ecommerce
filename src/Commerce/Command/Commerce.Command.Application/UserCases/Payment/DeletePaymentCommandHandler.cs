using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.Payment;
using Entities = Commerce.Command.Domain.Entities.Payment;
using MediatR;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;

namespace Commerce.Command.Application.UserCases.Payment
{
    /// <summary>
    /// Request to delete payment, contain payment id
    /// </summary>
    public record DeletePaymentCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete payment, contain payment id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete payment request
    /// </summary>
    public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, Result>
    {
        private readonly IPaymentRepository paymentRepository;

        /// <summary>
        /// Handler for delete payment request
        /// </summary>
        public DeletePaymentCommandHandler(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        /// <summary>
        /// Handle delete payment request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await paymentRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete payment
                var payment = await paymentRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (payment == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Payment)) })));
                }
                payment.IsDeleted = false;
                paymentRepository.Update(payment);
                await paymentRepository.SaveChangesAsync(cancellationToken);

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