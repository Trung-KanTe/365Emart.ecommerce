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
    public record CreateOrderCommand : IRequest<Result<Entities.Order>>
    {
        public Guid? UserId { get; set; }
        public Guid? PromotionId { get; set; }
        public decimal? TotalAmount { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }

    /// <summary>
    /// Handler for create order request
    /// </summary>
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<Entities.Order>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IPromotionRepository promotionRepository;
        private readonly ICartRepository cartRepository;
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly IProductStockRepository productStockRepository;
        private readonly IUserRepository userRepository;
        private readonly IEmailSender emailSender;


        /// <summary>
        /// Handler for create Order request
        /// </summary>
        public CreateOrderCommandHandler(IOrderRepository orderRepository, IPromotionRepository promotionRepository, ICartRepository cartRepository, IProductRepository productRepository, IProductDetailRepository productDetailRepository, IProductStockRepository productStockRepository, IUserRepository userRepository, IEmailSender emailSender)
        {
            this.orderRepository = orderRepository;
            this.promotionRepository = promotionRepository;
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
            this.productStockRepository = productStockRepository;
            this.userRepository = userRepository;
            this.emailSender = emailSender;
        }

        /// <summary>
        /// Handle create order request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with order data</returns>
        public async Task<Result<Entities.Order>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Create new Order from request
            Entities.Order? order = request.MapTo<Entities.Order>();
            // Validate for order
            order!.ValidateCreate();

            var cart = await cartRepository.FindSingleAsync(x => x.UserId == request.UserId!.Value, true, cancellationToken, x => x.CartItems!);

            // Lấy danh sách ProductDetailId từ giỏ hàng
            var productDetailIds = cart.CartItems.Select(x => x.ProductDetailId).ToList();

            // Lấy danh sách ProductDetail từ repository
            var productDetails = productDetailRepository.FindAll(x => productDetailIds.Contains(x.Id)).ToList();           

            // Lấy danh sách ProductId từ ProductDetail
            var productIds = productDetails.Where(pd => pd.ProductId.HasValue)
                                           .Select(pd => pd.ProductId!.Value)
                                           .Distinct()
                                           .ToList();

            // Lấy danh sách ShopId từ Product
            var shopIds = productRepository.FindAll(x => productIds.Contains(x.Id))
                                                 .Select(x => x.ShopId)
                                                 .Distinct()
                                                 .ToList();

            if (shopIds.Count > 1)
            {
                return Result.Failure(StatusCode.BadRequest, new Error(
                        ErrorType.ValidationProblem,
                        "DIFFERENT_SHOP",
                        "Các sản phẩm trong giỏ hàng phải thuộc cùng một cửa hàng."
                    ));
            }


            // Begin transaction
            using var transaction = await orderRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                order!.OrderItems = cart.CartItems!.Select(cartItem => new Entities.OrderItem
                {
                    OrderId = order.Id,
                    ProductDetailId = cartItem.ProductDetailId,
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity,
                    Total = cartItem.Total,
                }).ToList();
                if (request.PromotionId is not null)
                {
                    var promotion = await promotionRepository.FindByIdAsync(request.PromotionId!.Value, true, cancellationToken);
                    if (promotion == null)
                    {
                        return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entiti.Promotion)) })));
                    }
                }

                foreach (var item in order.OrderItems)
                {
                    var stockExist = await productStockRepository.FindSingleAsync(x => x.ProductDetailId == item.ProductDetailId, true, cancellationToken);
                    stockExist.Quantity -= item.Quantity;
                    productStockRepository.Update(stockExist);

                    var stockProduct = await productDetailRepository.FindByIdAsync(item.ProductDetailId!.Value, true, cancellationToken);
                    stockProduct!.StockQuantity -= item.Quantity;
                    productDetailRepository.Update(stockProduct);
                }

                cart.TotalQuantity = 0;
                cart.CartItems!.Clear();
                cartRepository.Update(cart);
                // Add data
                orderRepository.Create(order!);
                // Save data
                await orderRepository.SaveChangesAsync(cancellationToken);
                await productStockRepository.SaveChangesAsync(cancellationToken);
                await productDetailRepository.SaveChangesAsync(cancellationToken);

                var user = await userRepository.FindByIdAsync(request.UserId!.Value, true, cancellationToken);
                BackgroundJob.Enqueue(() => emailSender.SendOrderConfirmationEmailAsync(user!.Email!, order.Id));
                // Commit transaction
                transaction.Commit();
                return order;
            }
            catch (Exception)
            {
                // Rollback transaction
                transaction.Rollback();
                throw;
            }
        }
    }
}