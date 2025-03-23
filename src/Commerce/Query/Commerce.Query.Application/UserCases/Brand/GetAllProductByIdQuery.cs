using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Brand
{
    /// <summary>
    /// Request to get brand by id
    /// </summary>
    public record GetAllProductByIdQuery : IRequest<Result<PaginatedResult<ProductDTO>>>
    {
        public Guid? Id { get; init; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllProductByIdQuery(Guid? id, int pageNumber, int pageSize)
        {
            Id = id;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    /// <summary>
    /// Handler for get brand by id request
    /// </summary>
    /// <summary>
    /// Handle request
    /// </summary>
    /// <param name="request">Request to handle</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Result with brand data</returns>
    public class GetAllProductByIdQueryHandler : IRequestHandler<GetAllProductByIdQuery, Result<PaginatedResult<ProductDTO>>>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;

        public GetAllProductByIdQueryHandler(IProductRepository productRepository, IProductDetailRepository productDetailRepository)
        {
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
        }

        public async Task<Result<PaginatedResult<ProductDTO>>> Handle(GetAllProductByIdQuery request, CancellationToken cancellationToken)
        {
            // Lấy danh sách sản phẩm theo BrandId và phân trang
            var paginatedProducts = await productRepository.GetPaginatedResultAsync(
                request.PageNumber,
                request.PageSize,
                predicate: product => product.BrandId == request.Id, // Lọc theo BrandId
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