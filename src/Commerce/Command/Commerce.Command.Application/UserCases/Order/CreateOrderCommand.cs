using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Cart;
using Commerce.Command.Domain.Abstractions.Repositories.Order;
using Commerce.Command.Domain.Abstractions.Repositories.Promotion;
using Commerce.Command.Domain.Entities.Cart;
using Commerce.Command.Domain.Entities.Order;
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


        /// <summary>
        /// Handler for create Order request
        /// </summary>
        public CreateOrderCommandHandler(IOrderRepository orderRepository, IPromotionRepository promotionRepository, ICartRepository cartRepository)
        {
            this.orderRepository = orderRepository;
            this.promotionRepository = promotionRepository;
            this.cartRepository = cartRepository;
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
                var promotion = await promotionRepository.FindByIdAsync(request.PromotionId!.Value, true, cancellationToken);
                if (promotion == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entiti.Promotion)) })));
                }
                cart.TotalQuantity = 0;
                cart.CartItems!.Clear();
                cartRepository.Update(cart);
                // Add data
                orderRepository.Create(order!);
                // Save data
                await orderRepository.SaveChangesAsync(cancellationToken);
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