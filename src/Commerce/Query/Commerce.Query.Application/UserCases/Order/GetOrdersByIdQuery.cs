using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Order;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using MediatR;

namespace Commerce.Query.Application.UserCases.Order
{
    /// <summary>
    /// Request to get order by id
    /// </summary>
    public record GetOrdersByIdQuery : IRequest<Result<OrderDTO>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get order by id request
    /// </summary>
    public class GetOrdersByIdQueryHandler : IRequestHandler<GetOrdersByIdQuery, Result<OrderDTO>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;

        /// <summary>
        /// Handler for get order by id request
        /// </summary>
        public GetOrdersByIdQueryHandler(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IProductRepository productRepository, IProductDetailRepository productDetailRepository)
        {
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
            this.productDetailRepository = productDetailRepository;
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with order data</returns>
        public async Task<Result<OrderDTO>> Handle(GetOrdersByIdQuery request,
                                           CancellationToken cancellationToken)
        {
            // Validate request
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();

            // Lấy thông tin đơn hàng
            var order = await orderRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);

            OrderDTO orderDto = order.MapTo<OrderDTO>()!;

            // Lấy danh sách OrderItems
            var orderItems = orderItemRepository.FindAll(x => x.OrderId == request.Id).ToList();

            // Duyệt từng OrderItem để lấy thông tin ProductDetail và Product
            orderDto.OrderItems = new List<OrderItemDTO>();
            foreach (var orderItem in orderItems)
            {
                var orderItemDto = orderItem.MapTo<OrderItemDTO>()!;

                if (orderItem.ProductDetailId.HasValue)
                {
                    // Tìm ProductDetail
                    var productDetail = await productDetailRepository.FindByIdAsync(orderItem.ProductDetailId.Value, true, cancellationToken);
                    if (productDetail != null)
                    {
                        orderItemDto.ProductDetails = productDetail.MapTo<ProductDetailDTO>()!;
                        orderItemDto.ProductId = productDetail.ProductId;

                        // Tìm Product
                        var product = await productRepository.FindByIdAsync(productDetail.ProductId!.Value, true, cancellationToken);
                        if (product != null)
                        {
                            orderItemDto.ProductName = product.Name;
                            orderItemDto.ProductDescription = product.Description;
                            orderItemDto.ProductImage = product.Image;
                        }
                    }
                }

                orderDto.OrderItems.Add(orderItemDto);
            }

            return orderDto;
        }
    }
}
