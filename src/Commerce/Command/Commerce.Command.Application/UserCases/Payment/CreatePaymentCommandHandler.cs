using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Payment;
using Commerce.Command.Domain.Entities.Payment;
using MediatR;
using Entities = Commerce.Command.Domain.Entities.Payment;

namespace Commerce.Command.Application.UserCases.Payment
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreatePaymentCommand : IRequest<Result<Entities.Payment>>
    {
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
    /// Handler for create payment request
    /// </summary>
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<Entities.Payment>>
    {
        private readonly IPaymentRepository paymentRepository;

        /// <summary>
        /// Handler for create payment request
        /// </summary>
        public CreatePaymentCommandHandler(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        /// <summary>
        /// Handle create payment request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with payment data</returns>
        public async Task<Result<Entities.Payment>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            // Create new Payment from request
            Entities.Payment? payment = request.MapTo<Entities.Payment>();
            // Validate for payment
            payment!.ValidateCreate();
            // Begin transaction
            using var transaction = await paymentRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                //payment!.PaymentDetails = request.PaymentDetails!.Select(detail => new Entities.PaymentDetails
                //{
                //    PaymentId = payment.Id,
                //    BankCode = detail.BankCode,
                //    BankName = detail.BankName,
                //    CardNumber = detail.CardNumber,
                //    Note = detail.Note,
                //    ExtraData = detail.ExtraData,
                //}).ToList();
                // Add data
                paymentRepository.Create(payment!);
                // Save data
                await paymentRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return payment;
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