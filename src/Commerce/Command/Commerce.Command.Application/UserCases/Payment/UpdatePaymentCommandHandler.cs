using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Entities = Commerce.Command.Domain.Entities.Payment;
using Commerce.Command.Domain.Abstractions.Repositories.Payment;
using MediatR;
using Commerce.Command.Domain.Entities.Payment;

namespace Commerce.Command.Application.UserCases.Payment
{
    /// <summary>
    /// Request to delete payment, contain payment id
    /// </summary>
    public record UpdatePaymentCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public Guid? OrderId { get; set; }
        public decimal? Amount { get; set; }
        public string? TransactionId { get; set; }
        public string? ReturnUrl { get; set; }
        public string? OrderInfo { get; set; }
        public string? IpAddress { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public string? ResponseCode { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public ICollection<PaymentDetails>? PaymentDetails { get; set; }
    }

    /// <summary>
    /// Handler for delete payment request
    /// </summary>
    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, Result>
    {
        private readonly IPaymentRepository paymentRepository;

        /// <summary>
        /// Handler for delete payment request
        /// </summary>
        public UpdatePaymentCommandHandler(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        /// <summary>
        /// Handle delete payment request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await paymentRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete payment
                var payment = await paymentRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (payment == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Payment)) })));
                }
                // Update payment, keep original data if request is null
                request.MapTo(payment, true);
                request.MapTo(payment, true);
                //payment!.PaymentDetails = request.PaymentDetails?.Distinct().Select(detail => new Entities.PaymentDetails
                //{
                //    PaymentId = payment.Id,
                //    BankCode = detail.BankCode,
                //    BankName = detail.BankName,
                //    CardNumber = detail.CardNumber,
                //    Note = detail.Note,
                //    ExtraData = detail.ExtraData,
                //}).ToList() ?? payment.PaymentDetails;
                // Mark payment as Updated state
                paymentRepository.Update(payment);
                // Save payment to database
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