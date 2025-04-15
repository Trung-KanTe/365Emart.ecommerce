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
    public record GetAllBestSellerQuery : IRequest<Result<BestSellerDTO>>
    {

        public GetAllBestSellerQuery()
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
    public class GetAllBestSellerQueryHandler : IRequestHandler<GetAllBestSellerQuery, Result<BestSellerDTO>>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly IOrderCancelRepository orderCancelRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IShopRepository shopRepository;
        private readonly IOrderItemRepository orderItemRepository;

        public GetAllBestSellerQueryHandler(IProductRepository productRepository, IOrderCancelRepository orderCancelRepository,
            IOrderRepository orderRepository, IProductDetailRepository productDetailRepository, IShopRepository shopRepository, IOrderItemRepository orderItemRepository)
        {
            this.productRepository = productRepository;
            this.orderCancelRepository = orderCancelRepository;
            this.orderRepository = orderRepository;
            this.productDetailRepository = productDetailRepository;
            this.shopRepository = shopRepository;
            this.orderItemRepository = orderItemRepository;
        }

        public async Task<Result<BestSellerDTO>> Handle(GetAllBestSellerQuery request, CancellationToken cancellationToken)
        {
            var products = productRepository.FindAll(null, true, x => x.ProductDetails!).ToList();
            var productDetailIds = products.SelectMany(p => p.ProductDetails!).Select(pd => pd.Id).ToList();

            var orderItems = orderItemRepository.FindAll(oi => productDetailIds.Contains(oi.ProductDetailId.Value), true).ToList();

            var orderIds = orderItems.Select(oi => oi.OrderId).Distinct().ToList();
            var orders = orderRepository.FindAll(o => orderIds.Contains(o.Id), true).ToList();

            var totalOrders = orders.Count;
            var totalRevenue = (int)orders.Sum(o => o.TotalAmount ?? 0);

            var productSales = orderItems
                .GroupBy(oi => oi.ProductDetailId)
                .Select(g => new
                {
                    ProductDetailId = g.Key,
                    OrderCount = g.Count(),
                    TotalAmount = g.Sum(oi => oi.Total)
                })
                .ToList();

            var productSellerList = products
                .SelectMany(p => p.ProductDetails!
                    .Where(pd => productSales.Any(ps => ps.ProductDetailId == pd.Id)) 
                    .Select(pd =>
                    {
                        var sales = productSales.First(s => s.ProductDetailId == pd.Id);
                        return new ProductSellerDTO
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Image = p.Image,
                            OrderCount = sales.OrderCount,
                            TotalAmount = sales.TotalAmount
                        };
                    }))
                .OrderByDescending(p => p.OrderCount) 
                .Take(10) 
                .ToList();

            var bestSellerDto = new BestSellerDTO
            {
                TotalOrder = totalOrders,
                TotalRevenue = totalRevenue,
                Products = productSellerList
            };

            return bestSellerDto;
        }
    }
}