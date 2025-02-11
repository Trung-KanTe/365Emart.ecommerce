using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Order;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Order;

namespace Commerce.Query.Application.UserCases.Order
{
    /// <summary>
    /// Request to get all order
    /// </summary>
    public class GetAllOrderQuery : IRequest<Result<List<OrderDTO>>>
    {
        //public SearchCommand? SearchCommand { get; set; }
    }

    /// <summary>
    /// Handler for get all order request
    /// </summary>
    public class GetAllOrderQueryHandler : IRequestHandler<GetAllOrderQuery, Result<List<OrderDTO>>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;

        /// <summary>
        /// Handler for get all order request
        /// </summary>
        public GetAllOrderQueryHandler(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IProductRepository productRepository, IProductDetailRepository productDetailRepository)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
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
            //var ordersQuery = orderRepository.FindAll().ApplySearch(request.SearchCommand!);
            List<Entities.Order> orders = orderRepository.FindAll().ToList();
            List<OrderDTO> orderDtos = new List<OrderDTO>();

            foreach (var order in orders)
            {
                OrderDTO orderDto = order.MapTo<OrderDTO>()!;
                var orderItems = orderItemRepository.FindAll(x => x.OrderId == orderDto.Id)
                    .ToList()
                    .Select(x => x.MapTo<OrderItemDTO>()!)
                    .ToList();

                foreach (var orderItem in orderItems)
                {
                    if (orderItem.ProductDetailId.HasValue)
                    {
                        // Lấy ProductDetail từ productDetailRepository
                        var productDetail = await productDetailRepository.FindByIdAsync(orderItem.ProductDetailId.Value, true, cancellationToken);
                        if (productDetail != null && productDetail.ProductId.HasValue)
                        {
                            // Lấy thông tin Product từ productRepository
                            var product = await productRepository.FindByIdAsync(productDetail.ProductId.Value, true, cancellationToken);
                            if (product != null)
                            {
                                // Gán ProductName vào OrderItem
                                orderItem.ProductName = product.Name; // Giả sử `Product` có trường `Name`
                            }
                        }
                    }
                }

                // Gán danh sách OrderItem vào OrderDTO
                orderDto.OrderItems = orderItems;
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }
    }
}
