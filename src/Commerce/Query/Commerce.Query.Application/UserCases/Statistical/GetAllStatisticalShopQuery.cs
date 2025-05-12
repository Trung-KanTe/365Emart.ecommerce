using Commerce.Query.Application.DTOs;
using Commerce.Query.Application.DTOs.Statistical;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.ImportProduct;
using Commerce.Query.Domain.Abstractions.Repositories.Order;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Shop;

namespace Commerce.Query.Application.UserCases.Shop
{
    /// <summary>
    /// Request to get brand by id
    /// </summary>
    public record GetAllStatisticalShopQuery : IRequest<Result<List<StoreDTO>>>
    {

        public int? Month { get; init; }
        public int? Year { get; init; }
        public Guid? UserId { get; init; }

        public GetAllStatisticalShopQuery(int? month, int? year, Guid? userId)
        {
            Month = month;
            Year = year;
            UserId = userId;
        }
    }

    /// Handle request
    /// </summary>
    /// <param name="request">Request to handle</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Result with brand data</returns>
    public class GetAllStatisticalShopQueryHandler : IRequestHandler<GetAllStatisticalShopQuery, Result<List<StoreDTO>>>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly IShopRepository shopRepository;
        private readonly IImportProductDetailRepository importProductDetailRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderItemRepository orderItemRepository;

        public GetAllStatisticalShopQueryHandler(
            IProductRepository productRepository,
            IProductDetailRepository productDetailRepository,
            IShopRepository shopRepository,
            IImportProductDetailRepository importProductDetailRepository,
            IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
            this.shopRepository = shopRepository;
            this.importProductDetailRepository = importProductDetailRepository;
        }

        public async Task<Result<List<StoreDTO>>> Handle(GetAllStatisticalShopQuery request, CancellationToken cancellationToken)
        {
            var shops = new List<Entities.Shop>();

            // Nếu có truyền UserId thì chỉ lấy shop của User đó
            if (request.UserId.HasValue)
            {
                var shop = await shopRepository.FindSingleAsync(x => x.UserId == request.UserId, true, cancellationToken);
                if (shop != null)
                {
                    shops.Add(shop);
                }
            }
            else
            {
                // Nếu không truyền UserId thì lấy tất cả shop
                shops = shopRepository.FindAll().ToList();
            }

            // Chỉ lấy đơn hoàn thành
            var completedOrders = orderRepository.FindAll(x => x.Status == "completed", includeProperties: x => x.OrderItems).ToList();

            if (request.Month.HasValue && request.Year.HasValue)
            {
                completedOrders = completedOrders
                    .Where(x => x.InsertedAt.HasValue &&
                                x.InsertedAt.Value.Month == request.Month.Value &&
                                x.InsertedAt.Value.Year == request.Year.Value)
                    .ToList();
            }

            var completedOrderIds = completedOrders.Select(x => x.Id).ToList();
            var orderItems = orderItemRepository.FindAll(oi => completedOrderIds.Contains(oi.OrderId.Value));
            var orderDict = completedOrders.ToDictionary(x => x.Id, x => x);

            var productDetails = productDetailRepository.FindAll();
            var products = productRepository.FindAll();
            var productDict = products.ToDictionary(x => x.Id, x => x);
            var importProductDetails = importProductDetailRepository.FindAll();

            var result = new List<StoreDTO>();

            foreach (var shop in shops)
            {
                var shopProductIds = products
                    .Where(p => p.ShopId == shop.Id)
                    .Select(p => p.Id)
                    .ToHashSet();

                var shopProductDetailIds = productDetails
                    .Where(pd => shopProductIds.Contains(pd.ProductId.Value))
                    .Select(pd => pd.Id)
                    .ToHashSet();

                var shopOrderItems = orderItems
                    .Where(oi => shopProductDetailIds.Contains(oi.ProductDetailId.Value))
                    .ToList();

                var totalOrders = shopOrderItems
                    .Select(oi => oi.OrderId)
                    .Distinct()
                    .Count();

                decimal grossRevenue = 0;
                decimal? cost = 0;

                foreach (var item in shopOrderItems)
                {
                    grossRevenue += item.Total.Value;

                    var importPrice = importProductDetails
                        .Where(i => i.ProductDetailId == item.ProductDetailId)
                        .FirstOrDefault()?.ImportPrice ?? 0;

                    cost += item.Quantity * importPrice;
                }

                var netRevenue = grossRevenue * 0.9m;
                var profit = netRevenue - cost;

                var relatedOrderIds = shopOrderItems
                     .Select(oi => oi.OrderId.Value)
                     .Distinct()
                     .ToList();

                var shopOrders = relatedOrderIds
                    .Where(orderDict.ContainsKey)
                    .Select(id => orderDict[id])
                    .ToList();

                var orderDTOs = shopOrders
                    .Select(o => new OrderDTO
                    {
                        Id = o.Id,
                        TotalAmount = o.TotalAmount,
                        PaymentMethod = o.PaymentMethod,
                        Status = o.Status,
                        InsertedAt = o.InsertedAt,
                    })
                    .ToList();

                result.Add(new StoreDTO
                {
                    Id = shop.Id,
                    Name = shop.Name,
                    Description = shop.Description,
                    Image = shop.Image,
                    Style = shop.Style,
                    TotalOrder = totalOrders,
                    TotalGrossRevenue = grossRevenue,
                    TotalNetRevenue = netRevenue,
                    TotalProfit = profit.Value,
                    OrderDTOs = orderDTOs
                });
            }

            return result;
        }
    }
}