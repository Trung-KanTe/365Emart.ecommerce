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
    public record GetAllRevenueQuery : IRequest<Result<RevenueDTO>>
    {
        public GetAllRevenueQuery()
        {
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
    public class GetAllRevenueQueryHandler : IRequestHandler<GetAllRevenueQuery, Result<RevenueDTO>>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly IOrderCancelRepository orderCancelRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IShopRepository shopRepository;
        private readonly IOrderItemRepository orderItemRepository;

        public GetAllRevenueQueryHandler(IProductRepository productRepository, IOrderCancelRepository orderCancelRepository,
            IOrderRepository orderRepository, IProductDetailRepository productDetailRepository, IShopRepository shopRepository, IOrderItemRepository orderItemRepository)
        {
            this.productRepository = productRepository;
            this.orderCancelRepository = orderCancelRepository;
            this.orderRepository = orderRepository;
            this.productDetailRepository = productDetailRepository;
            this.shopRepository = shopRepository;
            this.orderItemRepository = orderItemRepository;
        }

        public async Task<Result<RevenueDTO>> Handle(GetAllRevenueQuery request, CancellationToken cancellationToken)
        {
            var products = productRepository.FindAll(null, true, x => x.ProductDetails!).ToList();

            // Nhóm sản phẩm theo ShopId
            var productGroups = products.GroupBy(p => p.ShopId).ToDictionary(g => g.Key, g => g.ToList());

            // Lấy danh sách ProductDetailId
            var productDetailIds = products.SelectMany(p => p.ProductDetails!).Select(pd => pd.Id).ToList();

            // Lấy danh sách OrderItems
            var orderItems = orderItemRepository.FindAll(oi => productDetailIds.Contains(oi.ProductDetailId.Value), true).ToList();
            var orderIds = orderItems.Select(oi => oi.OrderId).Distinct().ToList();

            // Lấy danh sách Orders
            var orders = orderRepository.FindAll(o => orderIds.Contains(o.Id), true).ToList();

            // Tổng số đơn hàng
            var totalOrders = orders.Count;
            var totalPending = orders.Count(o => o.Status == "pending");
            var totalConfirm = orders.Count(o => o.Status == "confirmed");
            var totalCompleted = orders.Count(o => o.Status == "completed");
            var totalCancel = orders.Count(o => o.Status == "canceled");

            // Tổng doanh thu
            var totalRevenue = orders.Sum(o => (int)o.TotalAmount);
            var totalRevenueProfit = (int)(totalRevenue * 0.3);

            // Doanh thu theo từng ShopId
            var revenueByShop = orders
                .GroupBy(o => o.Id)
                .Select(g => new ShopRevenueDTO
                {
                    Id = g.Key,
                    TotalRevenue = g.Sum(o => (int)o.TotalAmount),
                    TotalRevenueProfit = (int)(g.Sum(o => (int)o.TotalAmount) * 0.3)
                })
                .ToList();

            // Tạo DTO kết quả
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
                RevenueByShop = revenueByShop
            };

            return revenueDto;
        }
    }
}