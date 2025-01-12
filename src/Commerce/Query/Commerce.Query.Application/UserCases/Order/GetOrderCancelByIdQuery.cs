using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Order;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Order;

namespace Commerce.Query.Application.UserCases.Order
{
    /// <summary>
    /// Request to get orderCancel by id
    /// </summary>
    public record GetOrderCancelByIdQuery : IRequest<Result<Entities.OrderCancel>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get orderCancel by id request
    /// </summary>
    public class GetOrderCancelByIdQueryHandler : IRequestHandler<GetOrderCancelByIdQuery, Result<Entities.OrderCancel>>
    {
        private readonly IOrderCancelRepository orderCancelRepository;

        /// <summary>
        /// Handler for get orderCancel by id request
        /// </summary>
        public GetOrderCancelByIdQueryHandler(IOrderCancelRepository orderCancelRepository)
        {
            this.orderCancelRepository = orderCancelRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with orderCancel data</returns>
        public async Task<Result<Entities.OrderCancel>> Handle(GetOrderCancelByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            // Find orderCancel without allow null return. If orderCancel not found will throw NotFoundException
            var orderCancel = await orderCancelRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            return orderCancel!;
        }
    }
}
