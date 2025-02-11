using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Helpers;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Cart;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Cart;

namespace Commerce.Query.Application.UserCases.Cart
{
    /// <summary>
    /// Request to get all cart
    /// </summary>
    public class GetAllCartQuery : IRequest<Result<List<CartDTO>>>
    {

    }

    /// <summary>
    /// Handler for get all cart request
    /// </summary>
    public class GetAllCartQueryHandler : IRequestHandler<GetAllCartQuery, Result<List<CartDTO>>>
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartItemRepository cartItemRepository;

        /// <summary>
        /// Handler for get all cart request
        /// </summary>
        public GetAllCartQueryHandler(ICartRepository cartRepository, ICartItemRepository cartItemRepository)
        {
            this.cartRepository = cartRepository;
            this.cartItemRepository = cartItemRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list cart as data</returns>
        public async Task<Result<List<CartDTO>>> Handle(GetAllCartQuery request,
                                                       CancellationToken cancellationToken)
        {

            var carts = cartRepository.FindAll().ToList();
            List<CartDTO> orderDtos = carts.Select(order =>
            {
                CartDTO orderDto = order.MapTo<CartDTO>()!;
                orderDto.CartItems = cartItemRepository.FindAll(x => x.CartId == order.Id).ToList().Select(orderItem => orderItem.MapTo<Entities.CartItem>()!).ToList();
                return orderDto;
            }).ToList();

            return orderDtos;
        }
    }
}
