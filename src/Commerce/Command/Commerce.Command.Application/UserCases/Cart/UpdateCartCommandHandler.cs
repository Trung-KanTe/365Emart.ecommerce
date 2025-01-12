using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Entities = Commerce.Command.Domain.Entities.Cart;
using Commerce.Command.Domain.Abstractions.Repositories.Cart;
using MediatR;
using Commerce.Command.Domain.Entities.Cart;

namespace Commerce.Command.Application.UserCases.Cart
{
    /// <summary>
    /// Request to delete cart, contain cart id
    /// </summary>
    public record UpdateCartCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public int? TotalQuantity { get; set; } = 0;
        public bool? IsDeleted { get; set; } = false;
        public ICollection<CartItem>? CartItems { get; set; }
    }

    /// <summary>
    /// Handler for delete cart request
    /// </summary>
    public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, Result>
    {
        private readonly ICartRepository cartRepository;

        /// <summary>
        /// Handler for delete cart request
        /// </summary>
        public UpdateCartCommandHandler(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        /// <summary>
        /// Handle delete cart request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await cartRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete cart
                var cart = await cartRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (cart == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Cart)) })));
                }
                // Update cart, keep original data if request is null
                request.MapTo(cart, true);
                cart!.CartItems = request.CartItems!.Distinct().Select(ver => new Entities.CartItem
                {
                    CartId = cart.Id,
                    ProductId = ver.ProductId,
                    Price = ver.Price,
                    Quantity = ver.Quantity,
                    Total = ver.Price * ver.Quantity,
                }).ToList() ?? cart.CartItems;

                cart.TotalQuantity = cart.CartItems!.Count;
                // Mark cart as Updated state
                cartRepository.Update(cart);
                // Save cart to database
                await cartRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return Result.Ok();
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