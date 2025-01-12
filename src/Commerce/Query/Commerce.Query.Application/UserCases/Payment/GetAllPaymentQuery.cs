using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Helpers;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Payment;
using MediatR;
using Query.DTOs;
using Entities = Commerce.Query.Domain.Entities.Payment;

namespace Commerce.Query.Application.UserCases.Payment
{
    /// <summary>
    /// Request to get all payment
    /// </summary>
    public class GetAllPaymentQuery : IRequest<Result<List<PaymentDTO>>>
    {
        public SearchCommand? SearchCommand { get; set; }
    }

    /// <summary>
    /// Handler for get all payment request
    /// </summary>
    public class GetAllPaymentQueryHandler : IRequestHandler<GetAllPaymentQuery, Result<List<PaymentDTO>>>
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IPaymentDetailRepository paymentDetailRepository;

        /// <summary>
        /// Handler for get all payment request
        /// </summary>
        public GetAllPaymentQueryHandler(IPaymentRepository paymentRepository, IPaymentDetailRepository paymentDetailRepository)
        {
            this.paymentRepository = paymentRepository;
            this.paymentDetailRepository = paymentDetailRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list payment as data</returns>
        public async Task<Result<List<PaymentDTO>>> Handle(GetAllPaymentQuery request,
                                                       CancellationToken cancellationToken)
        {
            var paymentsQuery = paymentRepository.FindAll().ApplySearch(request.SearchCommand!);
            List<Entities.Payment> payments = paymentsQuery.ToList();
            List<PaymentDTO> paymentDtos = payments.Select(payment =>
            {
                PaymentDTO paymentDto = payment.MapTo<PaymentDTO>()!;
                paymentDto.PaymentDetails = paymentDetailRepository.FindAll(x => x.PaymentId == payment.Id).ToList().Select(paymentItem => paymentItem.MapTo<Entities.PaymentDetails>()!).ToList();
                return paymentDto;
            }).ToList();

            return paymentDtos;
        }
    }
}
