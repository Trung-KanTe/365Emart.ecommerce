using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
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

        /// <summary>
        /// Handler for get product by id request
        /// </summary>
        public GetProductByIdQueryHandler(IProductRepository productRepository, IProductDetailRepository productDetailRepository)
        {
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
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
            ProductDTO? orderDto = product!.MapTo<ProductDTO>()!;
            orderDto.ProductDetails = productDetailRepository.FindAll(x => x.ProductId == product.Id).ToList().Select(orderItem => orderItem.MapTo<Entities.ProductDetail>()!).ToList();
            return orderDto;
        }
    }
}
