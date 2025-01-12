using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Cart;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Cart;

namespace Commerce.Query.Application.UserCases.Cart
{
    /// <summary>
    /// Request to get cart by id
    /// </summary>
    public record GetCartByIdQuery : IRequest<Result<CartDTO>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get cart by id request
    /// </summary>
    public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, Result<CartDTO>>
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartItemRepository cartItemRepository;

        /// <summary>
        /// Handler for get cart by id request
        /// </summary>
        public GetCartByIdQueryHandler(ICartRepository cartRepository, ICartItemRepository cartItemRepository)
        {
            this.cartRepository = cartRepository;
            this.cartItemRepository = cartItemRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with cart data</returns>
        public async Task<Result<CartDTO>> Handle(GetCartByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            var cart = await cartRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            CartDTO cartDto = cart!.MapTo<CartDTO>()!;
            cartDto.CartItems = cartItemRepository.FindAll(x => x.CartId == request.Id).ToList().Select(x => x.MapTo<Entities.CartItem>()!).ToList();
            return cartDto;
        }
    }
}
