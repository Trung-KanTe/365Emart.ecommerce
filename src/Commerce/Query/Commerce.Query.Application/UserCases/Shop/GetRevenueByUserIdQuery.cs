using Commerce.Query.Application.DTOs.Shop;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Order;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;

namespace Commerce.Query.Application.UserCases.Shop
{
    /// <summary>
    /// Request to get brand by id
    /// </summary>
    public record GetRevenueByUserIdQuery : IRequest<Result<RevenueDTO>>
    {
        public Guid? Id { get; init; }

        public GetRevenueByUserIdQuery(Guid? id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Handler for get brand by id request
    /// </summary>
    /// <summary>
    /// Handle request
    /// </summary>
    /// <param name="request">Request to handle</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Result with brand data</returns>
    public class GetRevenueByUserIdQueryHandler : IRequestHandler<GetRevenueByUserIdQuery, Result<RevenueDTO>>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly IOrderCancelRepository orderCancelRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IShopRepository shopRepository;
        private readonly IOrderItemRepository orderItemRepository;

        public GetRevenueByUserIdQueryHandler(IProductRepository productRepository, IOrderCancelRepository orderCancelRepository,
            IOrderRepository orderRepository, IProductDetailRepository productDetailRepository, IShopRepository shopRepository, IOrderItemRepository orderItemRepository)
        {
            this.productRepository = productRepository;
            this.orderCancelRepository = orderCancelRepository;
            this.orderRepository = orderRepository;
            this.productDetailRepository = productDetailRepository;
            this.shopRepository = shopRepository;
            this.orderItemRepository = orderItemRepository;
        }

        public async Task<Result<RevenueDTO>> Handle(GetRevenueByUserIdQuery request, CancellationToken cancellationToken)
        {
            var shop = await shopRepository.FindSingleAsync(x => x.UserId == request.Id, true, cancellationToken);
            var products = productRepository.FindAll(x => x.ShopId == shop.Id, true, x => x.ProductDetails!).ToList();
            var productIds = products.Select(p => p.Id).ToList();
            var productDetailIds = products.SelectMany(p => p.ProductDetails!).Select(pd => pd.Id).ToList();

            var orderItems = orderItemRepository.FindAll(oi => productDetailIds.Contains(oi.ProductDetailId.Value), true).ToList();
            var orderIds = orderItems.Select(oi => oi.OrderId).Distinct().ToList();
            var orders = orderRepository.FindAll(o => orderIds.Contains(o.Id), true).ToList();

            var totalOrders = orders.Count;
            var totalPending = orders.Count(o => o.Status == "pending");
            var totalConfirm = orders.Count(o => o.Status == "confirmed");
            var totalCompleted = orders.Count(o => o.Status == "completed");
            var totalCancel = orders.Count(o => o.Status == "canceled");

            var totalRevenue = orders.Sum(oi => (int)oi.TotalAmount)!;
            var totalRevenueProfit = (int)(totalRevenue * 0.3);

            var monthlyRevenue = Enumerable.Range(1, 12).ToDictionary(m => m, m => orders
                   .Where(o => o.InsertedAt.HasValue && o.InsertedAt.Value.Year == DateTime.UtcNow.Year && o.InsertedAt.Value.Month == m)
                   .Sum(o => (int)o.TotalAmount));

            var revenueDto = new RevenueDTO
            {
                TotalOrder = totalOrders,
                TotalPending = totalPending,
                TotalConfirmed = totalConfirm,
                TotalCompleted = totalCompleted,
                TotalCancel = totalCancel,
                TotalRevenue = totalRevenue,
                TotalRevenueProfit = totalRevenueProfit,
                TotalProduct = products.Count,
            };

            return revenueDto;
        }
    }
}