using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Category;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Product
{
    /// <summary>
    /// Request to get product by id
    /// </summary>
    public record GetProductByIdQuery : IRequest<Result<ProductDTO>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get product by id request
    /// </summary>
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDTO>>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IShopRepository shopRepository;

        /// <summary>
        /// Handler for get product by id request
        /// </summary>
        public GetProductByIdQueryHandler(IProductRepository productRepository, IProductDetailRepository productDetailRepository, ICategoryRepository categoryRepository, IShopRepository shopRepository)
        {
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
            this.categoryRepository = categoryRepository;
            this.shopRepository = shopRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with product data</returns>
        public async Task<Result<ProductDTO>> Handle(GetProductByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            // Find product without allow null return. If product not found will throw NotFoundException
            var product = await productRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            var category = await categoryRepository.FindByIdAsync(product!.CategoryId!.Value, false, cancellationToken);
            var shop = await shopRepository.FindByIdAsync(product.ShopId!.Value, false, cancellationToken);
            ProductDTO? orderDto = product!.MapTo<ProductDTO>()!;
            orderDto.Category = category!.MapTo<CatDTO>()!;
            orderDto.Shop = shop!.MapTo<ShopDTO>()!;
            orderDto.ProductDetails = productDetailRepository.FindAll(x => x.ProductId == product!.Id).ToList().Select(orderItem => orderItem.MapTo<Entities.ProductDetail>()!).ToList();
            return orderDto;
        }
    }
}
