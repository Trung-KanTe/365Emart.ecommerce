using Commerce.Command.Application.UserCases.DTOs;
using Commerce.Command.Contract.Abstractions;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Order;
using Commerce.Command.Domain.Abstractions.Repositories.ProducStock;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Commerce.Command.Domain.Abstractions.Repositories.Promotion;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using Hangfire;
using MediatR;
using Entities = Commerce.Command.Domain.Entities.Order;

namespace Commerce.Command.Application.UserCases.Order
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateOrderBuyBackCommand : IRequest<Result<OrderDTO>>
    {
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for create order request
    /// </summary>
    public class CreateOrderBuyBackCommandHandler : IRequestHandler<CreateOrderBuyBackCommand, Result<OrderDTO>>
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
        public CreateOrderBuyBackCommandHandler(IOrderRepository orderRepository, IPromotionRepository promotionRepository, IProductRepository productRepository, IProductDetailRepository productDetailRepository, IProductStockRepository productStockRepository, IUserRepository userRepository, IEmailSender emailSender)
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
        public async Task<Result<OrderDTO>> Handle(CreateOrderBuyBackCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken, x => x.OrderItems);

            OrderDTO orderDTO = new();
            Entities.Order orderNew = new();
            //orderNew.Id = Guid.NewGuid();
            orderNew.UserId = order.UserId;
            orderNew.TotalAmount = order.TotalAmount;
            orderNew.PromotionId = order.PromotionId;
            orderNew.Address = order.Address;

            var promotion = await promotionRepository.FindByIdAsync(order.PromotionId.Value, true, cancellationToken);
            orderDTO.DiscountValue = promotion.DiscountValue;

            // Begin transaction
            using var transaction = await orderRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                orderNew!.OrderItems = order.OrderItems!.Select(orderItem => new Entities.OrderItem
                {
                    OrderId = orderNew.Id,
                    ProductDetailId = orderItem.ProductDetailId,
                    Price = orderItem.Price,
                    Quantity = orderItem.Quantity,
                    Total = orderItem.Price * orderItem.Quantity,
                }).ToList();

                foreach (var item in orderNew.OrderItems)
                {
                    var stockExist = await productStockRepository.FindSingleAsync(x => x.ProductDetailId == item.ProductDetailId, true, cancellationToken);
                    stockExist.Quantity -= item.Quantity;
                    productStockRepository.Update(stockExist);

                    var stockProduct = await productDetailRepository.FindByIdAsync(item.ProductDetailId!.Value, true, cancellationToken);
                    stockProduct!.StockQuantity -= item.Quantity;
                    productDetailRepository.Update(stockProduct);
                }
                // Add data
                orderRepository.Create(orderNew!);
                // Save data
                await orderRepository.SaveChangesAsync(cancellationToken);
                await productStockRepository.SaveChangesAsync(cancellationToken);
                await productDetailRepository.SaveChangesAsync(cancellationToken);

                orderNew.MapTo(orderDTO, true);

                var user = await userRepository.FindByIdAsync(order.UserId!.Value, true, cancellationToken);
                BackgroundJob.Enqueue(() => emailSender.SendOrderConfirmationEmailAsync(user!.Email!, order.Id));
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
    }
}