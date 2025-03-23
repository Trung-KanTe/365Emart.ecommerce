using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Order;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Order;

namespace Commerce.Query.Application.UserCases.Order
{
    /// <summary>
    /// Request to get all order
    /// </summary>
    public class GetAllOrderOfShopQuery : IRequest<Result<List<OrderDTO>>>
    {
        public Guid? UserId { get; init; }
    }

    /// <summary>
    /// Handler for get all order request
    /// </summary>
    public class GetAllOrderOfShopQueryHandler : IRequestHandler<GetAllOrderOfShopQuery, Result<List<OrderDTO>>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IShopRepository shopRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;

        /// <summary>
        /// Handler for get all order request
        /// </summary>
        public GetAllOrderOfShopQueryHandler(IOrderRepository orderRepository, IShopRepository shopRepository, IOrderItemRepository orderItemRepository, IProductRepository productRepository, IProductDetailRepository productDetailRepository)
        {
            this.orderRepository = orderRepository;
            this.shopRepository = shopRepository;
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
        public async Task<Result<List<OrderDTO>>> Handle(GetAllOrderOfShopQuery request,
                                                       CancellationToken cancellationToken)
        {
            // 1. Tìm Shop của User
            var shop = await shopRepository.FindSingleAsync(x => x.UserId == request.UserId, true, cancellationToken);

            // 2. Lấy danh sách sản phẩm thuộc Shop
            var productIds = productRepository.FindAll(x => x.ShopId == shop.Id)
                                              .Select(x => x.Id)
                                              .ToHashSet(); 


            // 3. Lấy tất cả Order có chứa sản phẩm thuộc Shop
            var orders = orderRepository.FindAll().ToList();
            List<OrderDTO> orderDtos = new();

            foreach (var order in orders)
            {
                var orderItems = orderItemRepository.FindAll(x => x.OrderId == order.Id)
                    .Where(x => x.ProductDetailId.HasValue) 
                    .ToList();

                // Lọc OrderItem có Product thuộc Shop
                var validOrderItems = new List<OrderItemDTO>();
                foreach (var orderItem in orderItems)
                {
                    var productDetail = await productDetailRepository.FindByIdAsync(orderItem.ProductDetailId!.Value, true, cancellationToken);
                    if (productDetail != null && productDetail.ProductId.HasValue && productIds.Contains(productDetail.ProductId.Value))
                    {
                        var orderItemDto = orderItem.MapTo<OrderItemDTO>()!;
                        var product = await productRepository.FindByIdAsync(productDetail.ProductId.Value, true, cancellationToken);

                        if (product != null)
                        {
                            orderItemDto.ProductName = product.Name;
                            orderItemDto.ProductDescription = product.Description;
                            orderItemDto.ProductImage = product.Image;
                        }

                        orderItemDto.ProductDetails = productDetail.MapTo<ProductDetailDTO>();
                        validOrderItems.Add(orderItemDto);
                    }
                }

                // Nếu đơn hàng có ít nhất một OrderItem hợp lệ thì mới thêm vào danh sách kết quả
                if (validOrderItems.Any())
                {
                    var orderDto = order.MapTo<OrderDTO>()!;
                    orderDto.ShopName = shop.Name;
                    orderDto.OrderItems = validOrderItems;
                    orderDtos.Add(orderDto);
                }
            }

            return orderDtos;
        }
    }
}
