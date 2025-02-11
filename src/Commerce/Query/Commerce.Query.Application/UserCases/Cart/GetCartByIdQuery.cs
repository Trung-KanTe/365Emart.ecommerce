using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Cart;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Entities.Product;
using MediatR;
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
        public async Task<Result<CartDTO>> Handle(GetCartByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.UserId).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            var cart = await cartRepository.FindSingleAsync(x => x.UserId == request.UserId!.Value, false, cancellationToken, x => x.CartItems!);

            var cartItemsWithProductDetails = cart.CartItems!.Where(x => x.ProductDetailId.HasValue).ToList();

            // Xử lý từng CartItem đồng bộ
            foreach (var cartItem in cartItemsWithProductDetails)
            {
                // Lấy ProductDetail cho từng CartItem
                ProductDetail? productDetail = await productDetailRepository.FindByIdAsync(cartItem.ProductDetailId!.Value, true, cancellationToken);

                if (productDetail != null)
                {
                    // Lấy Product liên quan đến ProductDetail
                    Entity.Product? product = await productRepository.FindByIdAsync(productDetail.ProductId!.Value, true, cancellationToken);
                }

                // Gán thông tin ProductDetail đã cập nhật vào CartItem nếu cần
                cartItem.ProductDetails = productDetail;
            }

            // Tiếp tục xây dựng CartDTO
            CartDTO? orderDto = cart.MapTo<CartDTO>()!;
            orderDto.CartItems = cart.CartItems!.Select(mc =>
            {
                Entities.CartItem? merClass = mc.MapTo<Entities.CartItem>();
                merClass!.ProductDetails = new ProductDetail
                {
                    Id = mc.ProductDetailId!.Value,
                    ProductId = mc.ProductDetails?.ProductId, // Lấy thông tin từ ProductDetail
                    Size = mc.ProductDetails?.Size,
                    Color = mc.ProductDetails?.Color,
                    StockQuantity = mc.ProductDetails?.StockQuantity,
                };
                return merClass;
            }).ToList();

            return orderDto;
        }
    }
}
