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
using Entities = Commerce.Command.Domain.Entities.Order;

namespace Commerce.Command.Application.UserCases.Order
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateOrderNowCommand : IRequest<Result<Entities.Order>>
    {
        public Guid? UserId { get; set; }
        public Guid? PromotionId { get; set; }
        public decimal? TotalAmount { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }

    /// <summary>
    /// Handler for create order request
    /// </summary>
    public class CreateOrderNowCommandHandler : IRequestHandler<CreateOrderNowCommand, Result<Entities.Order>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IPromotionRepository promotionRepository;
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly IProductStockRepository productStockRepository;
        private readonly IUserRepository userRepository;
        private readonly IEmailSender emailSender;


        /// <summary>
        /// Handler for create Order request
        /// </summary>
        public CreateOrderNowCommandHandler(IOrderRepository orderRepository, IPromotionRepository promotionRepository, IProductRepository productRepository, IProductDetailRepository productDetailRepository, IProductStockRepository productStockRepository, IUserRepository userRepository, IEmailSender emailSender)
        {
            this.orderRepository = orderRepository;
            this.promotionRepository = promotionRepository;
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
        public async Task<Result<Entities.Order>> Handle(CreateOrderNowCommand request, CancellationToken cancellationToken)
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
                order.TotalAmount = order.OrderItems.Sum(item => item.Total) - 10000;

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