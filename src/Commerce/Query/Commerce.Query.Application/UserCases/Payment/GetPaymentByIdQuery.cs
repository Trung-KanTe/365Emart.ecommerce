using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Payment;
using MediatR;
using Query.DTOs;
using Entities = Commerce.Query.Domain.Entities.Payment;

namespace Commerce.Query.Application.UserCases.Payment
{
    /// <summary>
    /// Request to get payment by id
    /// </summary>
    public record GetPaymentByIdQuery : IRequest<Result<PaymentDTO>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get payment by id request
    /// </summary>
    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, Result<PaymentDTO>>
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IPaymentDetailRepository paymentDetailRepository;

        /// <summary>
        /// Handler for get payment by id request
        /// </summary>
        public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository, IPaymentDetailRepository paymentDetailRepository)
        {
            this.paymentRepository = paymentRepository;
            this.paymentDetailRepository = paymentDetailRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with payment data</returns>
        public async Task<Result<PaymentDTO>> Handle(GetPaymentByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            var payment = await paymentRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            PaymentDTO paymentDto = payment!.MapTo<PaymentDTO>()!;
            paymentDto.PaymentDetails = paymentDetailRepository.FindAll(x => x.PaymentId == request.Id).ToList().Select(x => x.MapTo<Entities.PaymentDetails>()!).ToList();
            return paymentDto;
        }
    }
}
