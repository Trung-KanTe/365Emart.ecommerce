using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Product
{
    public class GetAllProductsQuery : IRequest<Result<PaginatedResult<ProductDTO>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllProductsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<PaginatedResult<ProductDTO>>>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IProductDetailRepository productDetailRepository)
        {
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
        }

        public async Task<Result<PaginatedResult<ProductDTO>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var paginatedProducts = await productRepository.GetPaginatedResultAsync(
                request.PageNumber,
                request.PageSize,
                predicate: null,
                isTracking: false
            );

            var productDTOs = paginatedProducts.Items.Select(product =>
            {
                ProductDTO productDto = product.MapTo<ProductDTO>()!;
                productDto.ProductDetails = productDetailRepository.FindAll(x => x.ProductId == product.Id).ToList().Select(orderItem => orderItem.MapTo<Entities.ProductDetail>()!).ToList();
                return productDto;
            }).ToList();

            var result = new PaginatedResult<ProductDTO>(
                request.PageNumber,
                request.PageSize,
                paginatedProducts.TotalCount,
                productDTOs
            );

            return result;
        }
    }
}
