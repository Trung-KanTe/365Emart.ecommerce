using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Order;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.Order;

namespace Commerce.Query.Application.UserCases.Order
{
    public class GetAllOrderOfShopPagingQuery : IRequest<Result<PaginatedResult<OrderDTO>>>
    {
        public Guid? UserId { get; init; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;

        public GetAllOrderOfShopPagingQuery(int pageNumber, Guid? userId)
        {
            PageNumber = pageNumber;
            UserId = userId;
        }
    }

    public class GetAllOrderOfShopPagingQueryHandler : IRequestHandler<GetAllOrderOfShopPagingQuery, Result<PaginatedResult<OrderDTO>>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IShopRepository shopRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;

        public GetAllOrderOfShopPagingQueryHandler(
            IOrderRepository orderRepository,
            IShopRepository shopRepository,
            IOrderItemRepository orderItemRepository,
            IProductRepository productRepository,
            IProductDetailRepository productDetailRepository)
        {
            this.orderRepository = orderRepository;
            this.shopRepository = shopRepository;
            this.orderItemRepository = orderItemRepository;
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
        }

        public async Task<Result<PaginatedResult<OrderDTO>>> Handle(GetAllOrderOfShopPagingQuery request, CancellationToken cancellationToken)
        {
            var shop = await shopRepository.FindSingleAsync(x => x.UserId == request.UserId, true, cancellationToken);
            var productIds = productRepository.FindAll(x => x.ShopId == shop.Id).Select(x => x.Id).ToHashSet();

            var validProductDetailIds = productDetailRepository.FindAll(pd => pd.ProductId.HasValue && productIds.Contains(pd.ProductId.Value))
                                                   .Select(pd => pd.Id)
                                                   .ToHashSet();
            Expression<Func<Entities.Order, bool>> predicate = order =>
                order.OrderItems.Any(item =>
                    item.ProductDetailId.HasValue &&
                    validProductDetailIds.Contains(item.ProductDetailId.Value) // Kiểm tra nếu ProductDetailId hợp lệ
                );

            var paginatedOrders = await orderRepository.GetPaginatedResultAsync(
                request.PageNumber,
                request.PageSize,
                predicate,
                isTracking: false
            );

            var orderDTOs = new List<OrderDTO>();

            foreach (var order in paginatedOrders.Items)
            {
                var orderItems = orderItemRepository.FindAll(x => x.OrderId == order.Id && x.ProductDetailId.HasValue).ToList();
                var validOrderItems = new List<OrderItemDTO>();

                foreach (var orderItem in orderItems)
                {
                    var productDetail = await productDetailRepository.FindByIdAsync(orderItem.ProductDetailId!.Value, true, cancellationToken);
                    if (productDetail != null && productDetail.ProductId.HasValue && productIds.Contains(productDetail.ProductId.Value))
                    {
                        var orderItemDto = orderItem.MapTo<OrderItemDTO>();
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

                if (validOrderItems.Any())
                {
                    var orderDto = order.MapTo<OrderDTO>();
                    orderDto.ShopName = shop.Name;
                    orderDto.OrderItems = validOrderItems;
                    orderDTOs.Add(orderDto);
                }
            }

            var result = new PaginatedResult<OrderDTO>(
                request.PageNumber,
                request.PageSize,
                paginatedOrders.TotalCount,
                orderDTOs
            );

            return result;
        }
    }
}
