using Commerce.Command.Application.UserCases.DTOs;
using Commerce.Command.Contract.Abstractions;
using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Cart;
using Commerce.Command.Domain.Abstractions.Repositories.Category;
using Commerce.Command.Domain.Abstractions.Repositories.Order;
using Commerce.Command.Domain.Abstractions.Repositories.ProducStock;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Commerce.Command.Domain.Abstractions.Repositories.Promotion;
using Commerce.Command.Domain.Abstractions.Repositories.Shop;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using Commerce.Command.Domain.Entities.Order;
using Hangfire;
using MediatR;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Entities = Commerce.Command.Domain.Entities.Order;

namespace Commerce.Command.Application.UserCases.Order
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateOrderNowCommand : IRequest<Result<OrderDTO>>
    {
        public Guid? UserId { get; set; }
        public Guid? PromotionId { get; set; }
        public decimal? TotalAmount { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }

    /// <summary>
    /// Handler for create order request
    /// </summary>
    public class CreateOrderNowCommandHandler : IRequestHandler<CreateOrderNowCommand, Result<OrderDTO>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IPromotionRepository promotionRepository;
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly IProductStockRepository productStockRepository;
        private readonly IUserRepository userRepository;
        private readonly IEmailSender emailSender;
        private readonly IShopRepository shopRepository;
        private readonly ICategoryRepository categoryRepository;


        /// <summary>
        /// Handler for create Order request
        /// </summary>
        public CreateOrderNowCommandHandler(IOrderRepository orderRepository, ICategoryRepository categoryRepository, IShopRepository shopRepository, IPromotionRepository promotionRepository, IProductRepository productRepository, IProductDetailRepository productDetailRepository, IProductStockRepository productStockRepository, IUserRepository userRepository, IEmailSender emailSender)
        {
            this.orderRepository = orderRepository;
            this.promotionRepository = promotionRepository;
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
            this.productStockRepository = productStockRepository;
            this.userRepository = userRepository;
            this.emailSender = emailSender;
            this.categoryRepository = categoryRepository;
            this.shopRepository = shopRepository;
        }

        /// <summary>
        /// Handle create order request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with order data</returns>
        public async Task<Result<OrderDTO>> Handle(CreateOrderNowCommand request, CancellationToken cancellationToken)
        {
            // Create new Order from request
            Entities.Order? order = request.MapTo<Entities.Order>();
            // Validate for order
            order!.ValidateCreate();          

            // Begin transaction
            using var transaction = await orderRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                order!.OrderItems = order.OrderItems!.Select(orderItem => new Entities.OrderItem
                {
                    OrderId = order.Id,
                    ProductDetailId = orderItem.ProductDetailId,
                    Price = orderItem.Price,
                    Quantity = orderItem.Quantity,
                    Total = orderItem.Price * orderItem.Quantity,
                }).ToList();
                order.TotalAmount = order.OrderItems.Sum(item => item.Total) - 35000;

                foreach (var item in order.OrderItems)
                {
                    var stockExist = await productStockRepository.FindSingleAsync(x => x.ProductDetailId == item.ProductDetailId, true, cancellationToken);
                    stockExist.Quantity -= item.Quantity;
                    productStockRepository.Update(stockExist);

                    var stockProduct = await productDetailRepository.FindByIdAsync(item.ProductDetailId!.Value, true, cancellationToken);
                    stockProduct!.StockQuantity -= item.Quantity;
                    productDetailRepository.Update(stockProduct);
                }
                // Add data
                orderRepository.Create(order!);
                // Save data
                await orderRepository.SaveChangesAsync(cancellationToken);
                await productStockRepository.SaveChangesAsync(cancellationToken);
                await productDetailRepository.SaveChangesAsync(cancellationToken);

                var user = await userRepository.FindByIdAsync(request.UserId!.Value, true, cancellationToken);
                BackgroundJob.Enqueue(() => emailSender.SendOrderConfirmationEmailAsync(user!.Email!, order.Id));

                var firstItem = order.OrderItems.FirstOrDefault();

                var productDetails = await productDetailRepository.FindByIdAsync(firstItem.ProductDetailId!.Value, true, cancellationToken);
                var products = await productRepository.FindByIdAsync(productDetails.ProductId!.Value, true, cancellationToken);
                var shop = await shopRepository.FindByIdAsync(products.ShopId!.Value, true, cancellationToken);


                var fromAddress = ParseAddress(shop.Address);
                var toAddress = ParseAddress(user.Address);

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

                order.TotalAmount += shippingFee;

                orderRepository.Update(order!);
                // Save data
                await orderRepository.SaveChangesAsync(cancellationToken);

                OrderDTO? orderDTO = order.MapTo<OrderDTO>();
                orderDTO.Shipping = shippingFee;
                // Commit transaction
                transaction.Commit();
                return orderDTO;
            }
            catch (Exception)
            {
                // Rollback transaction
                transaction.Rollback();
                throw;
            }
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