using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Cart;
using Commerce.Command.Domain.Abstractions.Repositories.Settings;
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
        private readonly ISignManager signManager;

        /// <summary>
        /// Handler for create cart request
        /// </summary>
        public CreateCartCommandHandler(ICartRepository cartRepository, ISignManager signManager)
        {
            this.cartRepository = cartRepository;
            this.signManager = signManager;
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
            Entities.Cart? cartUser = new Entities.Cart();
            request.MapTo(cartUser, true);
            // Begin transaction
            using var transaction = await cartRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                cartUser = await cartRepository.FindSingleAsync(x => x.UserId == request.UserId.Value, true, cancellationToken);
                if (cartUser == null)
                {
                    cartUser = new Entities.Cart
                    {
                        UserId = request.UserId.Value,
                        CartItems = request.CartItems?.Select(ver => new Entities.CartItem
                        {
                            ProductDetailId = ver.ProductDetailId,
                            Price = ver.Price,
                            Quantity = ver.Quantity,
                            Total = ver.Price * ver.Quantity,
                        }).ToList() ?? new List<Entities.CartItem>()
                    };

                    cartUser.TotalQuantity = cartUser.CartItems.Count;
                    // Add data
                    cartRepository.Create(cartUser!);
                    // Save data
                    await cartRepository.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    cartUser!.CartItems = request.CartItems!.Distinct().Select(ver => new Entities.CartItem
                    {
                        CartId = cartUser.Id,
                        ProductDetailId = ver.ProductDetailId,
                        Price = ver.Price,
                        Quantity = ver.Quantity,
                        Total = ver.Price * ver.Quantity,
                    }).ToList() ?? cartUser.CartItems;

                    cartUser.TotalQuantity++;
                    // Mark cart as Updated state
                    cartRepository.Update(cartUser);
                    // Save data
                    await cartRepository.SaveChangesAsync(cancellationToken);
                }
                          
                // Commit transaction
                transaction.Commit();
                return cartUser;
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