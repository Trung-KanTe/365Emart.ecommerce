using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Cart;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using MediatR;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Commerce.Query.Domain.Abstractions.Repositories.Category;

namespace Commerce.Query.Application.UserCases.Cart
{
    /// <summary>
    /// Request to get cart by id
    /// </summary>
    public record GetCartByIdQuery : IRequest<Result<CartDTO>>
    {
        public Guid? UserId { get; init; }
    }

    /// <summary>
    /// Handler for get cart by id request
    /// </summary>
    public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, Result<CartDTO>>
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartItemRepository cartItemRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly IProductRepository productRepository;
        private readonly IShopRepository shopRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;

        /// <summary>
        /// Handler for get cart by id request
        /// </summary>
        public GetCartByIdQueryHandler(
                 ICartRepository cartRepository,
                 ICartItemRepository cartItemRepository,
                 IProductDetailRepository productDetailRepository,
                 IProductRepository productRepository,
                 IShopRepository shopRepository,
                 IUserRepository userRepository,
                 ICategoryRepository categoryRepository)
        {
            this.cartRepository = cartRepository;
            this.cartItemRepository = cartItemRepository;
            this.productDetailRepository = productDetailRepository;
            this.productRepository = productRepository;
            this.shopRepository = shopRepository;
            this.userRepository = userRepository;
            this.categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with cart data</returns>
        public async Task<Result<CartDTO>> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.UserId).NotNull().IsGuid();
            validator.Validate();

            var user = await userRepository.FindByIdAsync(request.UserId.Value, true, cancellationToken);
            var cart = await cartRepository.FindSingleAsync(
                x => x.UserId == request.UserId!.Value,
                false,
                cancellationToken,
                x => x.CartItems!);

            if (cart == null)
            {
                return Result.Ok(new CartDTO());
            }

            var cartDto = new CartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalQuantity = cart.TotalQuantity,
                ShopCarts = new List<ShopCartDTO>()
            };

            var shopGroups = new Dictionary<Guid, (ShopCartDTO ShopCart, int TotalWeight, AddressDto FromAddress)>();

            foreach (var cartItem in cart.CartItems!)
            {
                var cartItemDto = new CartItemDTO
                {
                    Id = cartItem.Id,
                    CartId = cartItem.CartId,
                    ProductDetailId = cartItem.ProductDetailId,
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity,
                    Total = cartItem.Total
                };

                Guid? shopId = null;
                string? shopName = null;

                if (cartItem.ProductDetailId.HasValue)
                {
                    var productDetail = await productDetailRepository.FindByIdAsync(cartItem.ProductDetailId.Value, true, cancellationToken);
                    if (productDetail != null)
                    {
                        var product = await productRepository.FindByIdAsync(productDetail.ProductId!.Value, true, cancellationToken);

                        cartItemDto.ProductDetails = new ProductDetailDTO
                        {
                            Id = productDetail.Id,
                            ProductId = productDetail.ProductId,
                            Size = productDetail.Size,
                            Color = productDetail.Color,
                            StockQuantity = productDetail.StockQuantity,
                        };

                        cartItemDto.ProductName = product?.Name;
                        cartItemDto.ProductImage = product?.Image;

                        shopId = product?.ShopId;
                        if (shopId.HasValue)
                        {
                            var shop = await shopRepository.FindByIdAsync(shopId.Value, true, cancellationToken);
                            shopName = shop?.Name;

                            var fromAddress = ParseAddress(shop.Address);
                            var category = await categoryRepository.FindByIdAsync(product.CategoryId!.Value);
                            int estimatedWeight = EstimateWeightByCategory(category.Name);
                            int weight = estimatedWeight * cartItem.Quantity.Value;

                            if (!shopGroups.ContainsKey(shopId.Value))
                            {
                                var shopCart = new ShopCartDTO
                                {
                                    ShopId = shopId,
                                    ShopName = shopName,
                                    CartItems = new List<CartItemDTO>()
                                };
                                shopGroups[shopId.Value] = (shopCart, 0, fromAddress);
                                cartDto.ShopCarts.Add(shopCart);
                            }

                            var current = shopGroups[shopId.Value];
                            current.TotalWeight += weight;
                            current.ShopCart.CartItems.Add(cartItemDto);
                            shopGroups[shopId.Value] = (current.ShopCart, current.TotalWeight, current.FromAddress);
                        }

                        cartItemDto.ShopId = shopId;
                        cartItemDto.ShopName = shopName;
                    }
                }
            }

            // Tính phí vận chuyển sau khi gom nhóm xong
            foreach (var (shopId, (shopCart, totalWeight, fromAddress)) in shopGroups)
            {
                var toAddress = ParseAddress(user.Address);
                var shippingFee = await CalculateShippingFee(fromAddress, toAddress, totalWeight);
                shopCart.Shipping = shippingFee;
            }

            return Result.Ok(cartDto);
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
