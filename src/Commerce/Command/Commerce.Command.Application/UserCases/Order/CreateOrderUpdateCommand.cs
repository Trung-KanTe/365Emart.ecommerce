using Commerce.Command.Contract.Abstractions;
using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Cart;
using Commerce.Command.Domain.Abstractions.Repositories.Order;
using Commerce.Command.Domain.Abstractions.Repositories.ProducStock;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Commerce.Command.Domain.Abstractions.Repositories.Promotion;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using Commerce.Command.Domain.Entities.Order;
using Hangfire;
using MediatR;
using Entiti = Commerce.Command.Domain.Entities.Promotion;
using Entities = Commerce.Command.Domain.Entities.Order;

namespace Commerce.Command.Application.UserCases.Order
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateOrderUpdateCommand : IRequest<Result<List<Entities.Order>>>
    {
        public Guid? UserId { get; set; }
        public Guid? PromotionId { get; set; }
        public decimal? TotalAmount { get; set; }
        public int Shipping {  get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<ShopShippingInfo>? Shops { get; set; }
    }

    public class ShopShippingInfo
    {
        public Guid ShopId { get; set; }
        public int Shipping { get; set; }
    }

    /// <summary>
    /// Handler for create order request
    /// </summary>
    public class CreateOrderUpdateCommandHandler : IRequestHandler<CreateOrderUpdateCommand, Result<List<Entities.Order>>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IPromotionRepository promotionRepository;
        private readonly ICartRepository cartRepository;
        private readonly ICartItemRepository cartItemRepository;
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly IProductStockRepository productStockRepository;
        private readonly IUserRepository userRepository;
        private readonly IEmailSender emailSender;


        /// <summary>
        /// Handler for create Order request
        /// </summary>
        public CreateOrderUpdateCommandHandler(ICartItemRepository cartItemRepository, IOrderRepository orderRepository, IPromotionRepository promotionRepository, ICartRepository cartRepository, IProductRepository productRepository, IProductDetailRepository productDetailRepository, IProductStockRepository productStockRepository, IUserRepository userRepository, IEmailSender emailSender)
        {
            this.orderRepository = orderRepository;
            this.promotionRepository = promotionRepository;
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
            this.productStockRepository = productStockRepository;
            this.userRepository = userRepository;
            this.emailSender = emailSender;
            this.cartItemRepository = cartItemRepository;
        }

        /// <summary>
        /// Handle create order request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with order data</returns>
        public async Task<Result<List<Entities.Order>>> Handle(CreateOrderUpdateCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId!.Value;

            var cart = await cartRepository.FindSingleAsync(
                x => x.UserId == userId,
                true,
                cancellationToken,
                x => x.CartItems!
            );

            if (cart is null || cart.CartItems is null || !cart.CartItems.Any())
                return Result.Failure(StatusCode.BadRequest, new Error(ErrorType.ValidationProblem, "CART_EMPTY", "Giỏ hàng trống"));

            var selectedDetailIds = request.OrderItems?.Select(x => x.ProductDetailId).ToList();
            if (selectedDetailIds is null || !selectedDetailIds.Any())
                return Result.Failure(StatusCode.BadRequest, new Error(ErrorType.ValidationProblem, "NO_ITEM_SELECTED", "Không có sản phẩm nào được chọn"));

            // Lọc CartItems theo productDetailId được chọn
            var selectedCartItems = cart.CartItems.Where(ci => selectedDetailIds.Contains(ci.ProductDetailId)).ToList();

            // Lấy ProductDetail kèm Product để truy cập ShopId
            var productDetailIds = selectedCartItems.Select(ci => ci.ProductDetailId!.Value).Distinct().ToList();
            var productDetails = productDetailRepository.FindAll(pd => productDetailIds.Contains(pd.Id)).ToList();

            var productIds = productDetails.Where(x => x.ProductId.HasValue).Select(x => x.ProductId!.Value).Distinct().ToList();
            var products = productRepository.FindAll(p => productIds.Contains(p.Id)).ToList();

            // Nhóm sản phẩm theo shopId
            var cartItemsGroupedByShop = selectedCartItems
                .GroupBy(ci =>
                {
                    var productDetail = productDetails.FirstOrDefault(pd => pd.Id == ci.ProductDetailId);
                    var product = products.FirstOrDefault(p => p.Id == productDetail?.ProductId);
                    return product?.ShopId ?? Guid.Empty;
                })
                .ToList();

            // Nếu có nhiều shop, tạo nhiều đơn hàng
            var createdOrders = new List<Entities.Order>();

            using var transaction = await orderRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                foreach (var group in cartItemsGroupedByShop)
                {
                    var groupTotal = group.Sum(x => x.Total ?? 0);

                    decimal discountAmount = 0;
                    if (request.PromotionId.HasValue)
                    {
                        var promotion = await promotionRepository.FindByIdAsync(request.PromotionId.Value, true, cancellationToken);
                        if (promotion != null && promotion.DiscountValue > 0)
                        {
                            discountAmount = promotion.DiscountValue;
                        }
                    }

                    // ✅ Xác định ShopId của group này
                    var firstItem = group.FirstOrDefault();
                    var productDetail = productDetails.FirstOrDefault(pd => pd.Id == firstItem?.ProductDetailId);
                    var product = products.FirstOrDefault(p => p.Id == productDetail?.ProductId);
                    var shopId = product?.ShopId ?? Guid.Empty;

                    // ✅ Lấy phí ship tương ứng từ request.Shops
                    var shopShipping = request.Shops?.FirstOrDefault(s => s.ShopId == shopId)?.Shipping ?? 0;

                    var order = new Entities.Order
                    {
                        UserId = userId,
                        PromotionId = request.PromotionId,
                        TotalAmount = groupTotal - discountAmount > 0 ? groupTotal - discountAmount + shopShipping : 0,
                        InsertedAt = DateTime.UtcNow,
                        OrderItems = new List<Entities.OrderItem>()
                    };

                    orderRepository.Create(order); // sinh Id ở đây

                    order.OrderItems = group.Select(ci => new Entities.OrderItem
                    {
                        OrderId = order.Id,
                        ProductDetailId = ci.ProductDetailId,
                        Quantity = ci.Quantity,
                        Price = ci.Price,
                        Total = ci.Total
                    }).ToList();

                    createdOrders.Add(order);

                    // Trừ tồn kho như cũ...
                    foreach (var item in order.OrderItems)
                    {
                        var stock = await productStockRepository.FindSingleAsync(x => x.ProductDetailId == item.ProductDetailId, true, cancellationToken);
                        stock.Quantity -= item.Quantity ?? 0;
                        productStockRepository.Update(stock);

                        var detail = await productDetailRepository.FindByIdAsync(item.ProductDetailId!.Value, true, cancellationToken);
                        detail.StockQuantity -= item.Quantity ?? 0;
                        productDetailRepository.Update(detail);
                    }
                }

                // Cập nhật cart: Xóa cartItems đã đặt hàng
                cart.CartItems = cart.CartItems
                    .Where(ci => !selectedDetailIds.Contains(ci.ProductDetailId))
                    .ToList();

                cart.TotalQuantity = cart.CartItems.Sum(x => x.Quantity ?? 0);
                cartRepository.Update(cart);

                await orderRepository.SaveChangesAsync(cancellationToken);
                await productStockRepository.SaveChangesAsync(cancellationToken);
                await productDetailRepository.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                return createdOrders; 
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

    }
}