using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Cart;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;

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
        private readonly IShopRepository shopRepository;

        /// <summary>
        /// Handler for get cart by id request
        /// </summary>
        public GetCartByIdQueryHandler(
                 ICartRepository cartRepository,
                 ICartItemRepository cartItemRepository,
                 IProductDetailRepository productDetailRepository,
                 IProductRepository productRepository,
                 IShopRepository shopRepository)
        {
            this.cartRepository = cartRepository;
            this.cartItemRepository = cartItemRepository;
            this.productDetailRepository = productDetailRepository;
            this.productRepository = productRepository;
            this.shopRepository = shopRepository;
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
                return Result.Ok(new CartDTO());
            }

            var cartDto = new CartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalQuantity = cart.TotalQuantity,
                ShopCarts = new List<ShopCartDTO>()
            };

            var shopGroups = new Dictionary<Guid, ShopCartDTO>();

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

                Guid? shopId = null;
                string? shopName = null;

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
                            Color = productDetail.Color,
                            StockQuantity = productDetail.StockQuantity,
                        };

                        cartItemDto.ProductName = product?.Name;
                        cartItemDto.ProductImage = product?.Image;

                        shopId = product?.ShopId;
                        if (shopId.HasValue)
                        {
                            var shop = await shopRepository.FindByIdAsync(shopId.Value, true, cancellationToken);
                            shopName = shop?.Name;
                        }

                        cartItemDto.ShopId = shopId;
                        cartItemDto.ShopName = shopName;
                    }
                }

                if (shopId.HasValue)
                {
                    if (!shopGroups.TryGetValue(shopId.Value, out var shopCart))
                    {
                        shopCart = new ShopCartDTO
                        {
                            ShopId = shopId,
                            ShopName = shopName
                        };
                        shopGroups[shopId.Value] = shopCart;
                        cartDto.ShopCarts.Add(shopCart);
                    }

                    shopGroups[shopId.Value].CartItems.Add(cartItemDto);
                }              
            }

            return Result.Ok(cartDto);
        }
    }
}
