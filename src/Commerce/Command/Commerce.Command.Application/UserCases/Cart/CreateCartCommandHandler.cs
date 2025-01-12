using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Cart;
using Commerce.Command.Domain.Entities.Cart;
using MediatR;
using Entities = Commerce.Command.Domain.Entities.Cart;

namespace Commerce.Command.Application.UserCases.Cart
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateCartCommand : IRequest<Result<Entities.Cart>>
    {
        public Guid? UserId { get; set; } 
        public int? TotalQuantity { get; set; } = 0;  
        public bool? IsDeleted { get; set; } = false;  
        public ICollection<CartItem>? CartItems { get; set; }
    }

    /// <summary>
    /// Handler for create cart request
    /// </summary>
    public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, Result<Entities.Cart>>
    {
        private readonly ICartRepository cartRepository;

        /// <summary>
        /// Handler for create cart request
        /// </summary>
        public CreateCartCommandHandler(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        /// <summary>
        /// Handle create cart request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with cart data</returns>
        public async Task<Result<Entities.Cart>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            // Create new Cart from request
            Entities.Cart? cart = request.MapTo<Entities.Cart>();
            // Validate for cart
            cart!.ValidateCreate();
            // Begin transaction
            using var transaction = await cartRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                cart!.CartItems = request.CartItems!.Distinct().Select(ver => new Entities.CartItem
                {
                    CartId = cart.Id,
                    ProductId = ver.ProductId,
                    Price = ver.Price,
                    Quantity = ver.Quantity,
                    Total = ver.Price * ver.Quantity,
                }).ToList();

                cart.TotalQuantity = cart.CartItems.Count;
                // Add data
                cartRepository.Create(cart!);
                // Save data
                await cartRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return cart;
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