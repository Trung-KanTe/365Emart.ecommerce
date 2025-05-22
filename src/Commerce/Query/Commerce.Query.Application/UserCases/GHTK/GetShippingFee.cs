using Commerce.Query.Application.DTOs.Statistical;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Category;
using Commerce.Query.Domain.Abstractions.Repositories.Order;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commerce.Query.Application.UserCases.GHTK
{
    /// <summary>
    /// Request to get all order
    /// </summary>
    public class GetShippingFee : IRequest<Result<ShippingFee>>
    {
        public List<Guid> OrderIds { get; init; }
        public string Province { get; init; }
        public string District { get; init; }
        public List<ShopShippingDto> Shops { get; init; }
    }

    public class ShopShippingDto
    {
        public Guid ShopId { get; set; }
        public int Shipping { get; set; }
    }

    /// <summary>
    /// Handler for get all order request
    /// </summary>
    public class GetShippingFeeHandler : IRequestHandler<GetShippingFee, Result<ShippingFee>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IShopRepository shopRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly ICategoryRepository categoryRepository;

        /// <summary>
        /// Handler for get all order request
        /// </summary>
        public GetShippingFeeHandler(IOrderRepository orderRepository, IShopRepository shopRepository, IOrderItemRepository orderItemRepository, IProductRepository productRepository, IProductDetailRepository productDetailRepository, ICategoryRepository categoryRepository)
        {
            this.orderRepository = orderRepository;
            this.shopRepository = shopRepository;
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
            this.orderItemRepository = orderItemRepository;
            this.categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list order as data</returns>
        public async Task<Result<ShippingFee>> Handle(GetShippingFee request,
                                                       CancellationToken cancellationToken)
        {
            var shopShippingFees = new Dictionary<Guid, int>();  // Sử dụng Dictionary để lưu trữ phí ship theo từng shop

            foreach (var orderId in request.OrderIds)
            {
                // Lấy đơn hàng
                var order = await orderRepository.FindByIdAsync(orderId, true, cancellationToken, includeProperties: x => x.OrderItems);

                // Lấy sản phẩm đầu tiên để xác định shop (giả sử 1 đơn chỉ từ 1 shop)
                var firstProductDetail = await productDetailRepository.FindByIdAsync(order.OrderItems.First().ProductDetailId.Value);
                var firstProduct = await productRepository.FindByIdAsync(firstProductDetail.ProductId.Value);
                var shop = await shopRepository.FindByIdAsync(firstProduct.ShopId.Value);

                var fromAddress = ParseAddress(shop.Address);
                var toAddress = new AddressDto
                {
                    Province = request.Province,
                    District = request.District,
                };

                // Tổng khối lượng đơn hàng
                int totalWeight = 0;
                foreach (var item in order.OrderItems)
                {
                    var productDetail = await productDetailRepository.FindByIdAsync(item.ProductDetailId.Value);
                    var product = await productRepository.FindByIdAsync(productDetail.ProductId.Value);
                    var category = await categoryRepository.FindByIdAsync(product.CategoryId!.Value);

                    int estimatedWeight = EstimateWeightByCategory(category.Name); // product.Category là string
                    totalWeight += estimatedWeight * item.Quantity!.Value;
                }

                // Gọi API tính phí của Giao Hàng Tiết Kiệm
                var shippingFee = await CalculateShippingFee(fromAddress, toAddress, totalWeight);

                // Cộng phí shipping cho shop tương ứng
                if (shopShippingFees.ContainsKey(shop.Id))
                {
                    shopShippingFees[shop.Id] += shippingFee;  // Cộng phí vào shop đã có
                }
                else
                {
                    shopShippingFees[shop.Id] = shippingFee;  // Thêm shop mới vào dictionary
                }
            }

            foreach (var orderId in request.OrderIds)
            {
                var order = await orderRepository.FindByIdAsync(
                    orderId,
                    true,
                    cancellationToken,
                    includeProperties: x => x.OrderItems);

                // Lấy shopId của order
                var firstProductDetail = await productDetailRepository.FindByIdAsync(
                    order.OrderItems.First().ProductDetailId.Value);
                var firstProduct = await productRepository.FindByIdAsync(firstProductDetail.ProductId.Value);
                var shopId = firstProduct.ShopId.Value;

                // Phí cũ từ request
                var oldShipping = request.Shops
                    .FirstOrDefault(s => s.ShopId == shopId)?.Shipping ?? 0;

                // Phí mới đã tính
                var newShipping = shopShippingFees.ContainsKey(shopId)
                    ? shopShippingFees[shopId]
                    : 0;

                // Cập nhật TotalAmount = TotalAmount cũ - oldShipping + newShipping
                order.TotalAmount = order.TotalAmount - oldShipping + newShipping;

                // Lưu lại thay đổi
                orderRepository.Update(order);
            }
            await orderRepository.SaveChangesAsync();

            // Tính tổng phí ship của tất cả các shop
            var totalShippingFee = shopShippingFees.Values.Sum();

            return new ShippingFee
            {
                Shipping = totalShippingFee  // Trả về tổng phí ship của tất cả các shop
            };
        }

        private async Task<int> CalculateShippingFee(AddressDto from, AddressDto to, int weight)
        {
            using var httpClient = new HttpClient();

            var url = "https://services.giaohangtietkiem.vn/services/shipment/fee";
            httpClient.DefaultRequestHeaders.Add("Token", "1V3kiA7S92Uecej5X8gwAX4NkHpmE2itwXdesZi");

            var queryParams = new Dictionary<string, string>
            {
                { "pick_province", from.Province },
                { "pick_district", from.District },
                { "province", to.Province },
                { "district", to.District },
                { "weight", (weight).ToString() },
                { "deliver_option", "none" }
            };

            var requestUrl = $"{url}?{string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"))}";
            var response = await httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
                return 0;

            var responseData = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<JObject>(responseData);

            if (json["fee"] is JObject feeObj && feeObj["fee"] != null)
            {
                return feeObj["fee"].Value<int>();
            }
            return 0;
        }

        public class AddressDto
        {
            public string Province { get; set; }
            public string District { get; set; }
            public string Ward { get; set; }
        }

        private AddressDto ParseAddress(string addressString)
        {
            var parts = addressString.Split(',')
                                      .Select(p => p.Trim())
                                      .ToList();

            return new AddressDto
            {
                Ward = RemoveAdministrativePrefix(parts.ElementAtOrDefault(0)),
                District = RemoveAdministrativePrefix(parts.ElementAtOrDefault(1)),
                Province = RemoveAdministrativePrefix(parts.ElementAtOrDefault(2))
            };
        }

        private string RemoveAdministrativePrefix(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

                    var prefixes = new[] {
                "Tỉnh ", "Thành phố ", "TP. ", "TP ", "Quận ", "Huyện ", "Thị xã ", "Xã ", "Phường ", "Thị trấn "
            };

            foreach (var prefix in prefixes)
            {
                if (input.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    return input.Substring(prefix.Length).Trim();
                }
            }

            return input.Trim();
        }

        private int EstimateWeightByCategory(string category)
        {
            return category switch
            {
                "Mens_Clothing" => 500,
                "Womens_Clothing" => 400,
                "Childrens_Clothing" => 300,
                "Olds_Clothing" => 550,
                "Foot_Wear" => 900,
                "Kitchen_Ware" => 1200,
                "Skincare" => 200,
                "Beverages" => 1500,
                _ => 500 
            };
        }
    }
}