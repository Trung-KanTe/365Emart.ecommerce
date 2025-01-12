using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Helpers;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Order;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Order;

namespace Commerce.Query.Application.UserCases.Order
{
    /// <summary>
    /// Request to get all order
    /// </summary>
    public class GetAllOrderQuery : IRequest<Result<List<OrderDTO>>>
    {
        public SearchCommand? SearchCommand { get; set; }
    }

    /// <summary>
    /// Handler for get all order request
    /// </summary>
    public class GetAllOrderQueryHandler : IRequestHandler<GetAllOrderQuery, Result<List<OrderDTO>>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderItemRepository orderItemRepository;

        /// <summary>
        /// Handler for get all order request
        /// </summary>
        public GetAllOrderQueryHandler(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list order as data</returns>
        public async Task<Result<List<OrderDTO>>> Handle(GetAllOrderQuery request,
                                                       CancellationToken cancellationToken)
        {
            var ordersQuery = orderRepository.FindAll().ApplySearch(request.SearchCommand!);
            List<Entities.Order> orders = ordersQuery.ToList();
            List<OrderDTO> orderDtos = orders.Select(order =>
            {
                OrderDTO orderDto = order.MapTo<OrderDTO>()!;
                orderDto.OrderItems = orderItemRepository.FindAll(x => x.OrderId == order.Id).ToList().Select(orderItem => orderItem.MapTo<Entities.OrderItem>()!).ToList();
                return orderDto;
            }).ToList();

            return orderDtos;
        }
    }
}
