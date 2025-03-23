using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Cart;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Entities.Product;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Entities = Commerce.Query.Domain.Entities.Cart;
using Entity = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Cart
{
    /// <summary>
    /// Request to get cart by id
    /// </summary>
    public record GetCartByIdQuery : IRequest<Result<CartDTO>>
    {
        public Guid? UserId { get; init; }
    }

    /// <summary>
    /// Handler for get cart by id request
    /// </summary>
    public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, Result<CartDTO>>
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartItemRepository cartItemRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly IProductRepository productRepository;

        /// <summary>
        /// Handler for get cart by id request
        /// </summary>
        public GetCartByIdQueryHandler(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IProductDetailRepository productDetailRepository, IProductRepository productRepository)
        {
            this.cartRepository = cartRepository;
            this.cartItemRepository = cartItemRepository;
            this.productDetailRepository = productDetailRepository;
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with cart data</returns>
        public async Task<Result<CartDTO>> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.UserId).NotNull().IsGuid();
            validator.Validate();

            var cart = await cartRepository.FindSingleAsync(
                x => x.UserId == request.UserId!.Value,
                false,
                cancellationToken,
                x => x.CartItems!);

            if (cart == null)
            {
                return Result.Ok(new CartDTO()); // Trả về giỏ hàng rỗng nếu không tìm thấy
            }

            var cartDto = new CartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalQuantity = cart.TotalQuantity,
                CartItems = new List<CartItemDTO>()
            };

            foreach (var cartItem in cart.CartItems!)
            {
                var cartItemDto = new CartItemDTO
                {
                    Id = cartItem.Id,
                    CartId = cartItem.CartId,
                    ProductDetailId = cartItem.ProductDetailId,
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity,
                    Total = cartItem.Total
                };

                if (cartItem.ProductDetailId.HasValue)
                {
                    var productDetail = await productDetailRepository.FindByIdAsync(cartItem.ProductDetailId.Value, true, cancellationToken);
                    if (productDetail != null)
                    {
                        var product = await productRepository.FindByIdAsync(productDetail.ProductId!.Value, true, cancellationToken);

                        cartItemDto.ProductDetails = new ProductDetailDTO
                        {
                            Id = productDetail.Id,
                            ProductId = productDetail.ProductId,
                            Size = productDetail.Size,
                            Color = productDetail.Color
                        };

                        cartItemDto.ProductName = product?.Name;
                        cartItemDto.ProductImage = product?.Image;
                    }
                }

                cartDto.CartItems.Add(cartItemDto);
            }

            return Result.Ok(cartDto);
        }
    }
}
