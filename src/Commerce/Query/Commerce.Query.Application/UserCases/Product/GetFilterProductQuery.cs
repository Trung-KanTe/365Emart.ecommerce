using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Helpers;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Product
{
    /// <summary>
    /// Request to get all product
    /// </summary>
    public class GetFilterProductQuery : IRequest<Result<PaginatedResult<ProductDTO>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public SearchCommand? SearchCommand { get; set; }
    }

    /// <summary>
    /// Handler for get all product request
    /// </summary>
    public class GetFilterProductQueryHandler : IRequestHandler<GetFilterProductQuery, Result<PaginatedResult<ProductDTO>>>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;

        public GetFilterProductQueryHandler(IProductRepository productRepository, IProductDetailRepository productDetailRepository)
        {
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
        }

        public async Task<Result<PaginatedResult<ProductDTO>>> Handle(GetFilterProductQuery request, CancellationToken cancellationToken)
        {
            var productsQuery = productRepository.FindAll().ApplySearch(request.SearchCommand!);

            // Lấy tổng số sản phẩm sau khi áp dụng filter
            int totalCount = productsQuery.Count();

            // Kiểm tra nếu trang không hợp lệ (quá lớn so với dữ liệu)
            if (totalCount == 0 || (request.PageNumber - 1) * request.PageSize >= totalCount)
            {
                return new PaginatedResult<ProductDTO>(request.PageNumber, request.PageSize, totalCount, new List<ProductDTO>());
            }

            // Áp dụng phân trang đúng cách
            var pagedProducts = productsQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Chuyển đổi sang DTO
            var productDtos = pagedProducts.Select(product =>
            {
                var productDto = product.MapTo<ProductDTO>()!;
                productDto.ProductDetails = productDetailRepository
                    .FindAll(x => x.ProductId == product.Id)
                    .ToList()
                    .Select(orderItem => orderItem.MapTo<Entities.ProductDetail>()!)
                    .ToList();
                return productDto;
            }).ToList();

            // Trả về dữ liệu phân trang
            var paginatedResult = new PaginatedResult<ProductDTO>(request.PageNumber, request.PageSize, totalCount, productDtos);
            return paginatedResult;
        }
    }
}
