using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Order;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Order;

namespace Commerce.Query.Application.UserCases.Order
{
    /// <summary>
    /// Request to get order by id
    /// </summary>
    public record GetOrderByIdQuery : IRequest<Result<OrderDTO>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get order by id request
    /// </summary>
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderDTO>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderItemRepository orderItemRepository;

        /// <summary>
        /// Handler for get order by id request
        /// </summary>
        public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with order data</returns>
        public async Task<Result<OrderDTO>> Handle(GetOrderByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            var Order = await orderRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            OrderDTO OrderDto = Order!.MapTo<OrderDTO>()!;
            OrderDto.OrderItems = orderItemRepository.FindAll(x => x.OrderId == request.Id).ToList().Select(x => x.MapTo<Entities.OrderItem>()!).ToList();
            return OrderDto;
        }
    }
}
