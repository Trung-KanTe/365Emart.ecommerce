using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Brand;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Category
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
        private readonly IBrandRepository brandRepository;

        public GetAllProductByIdQueryHandler(IProductRepository productRepository, IProductDetailRepository productDetailRepository, IBrandRepository brandRepository)
        {
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
            this.brandRepository = brandRepository;
        }

        public async Task<Result<PaginatedResult<ProductDTO>>> Handle(GetAllProductByIdQuery request, CancellationToken cancellationToken)
        {
            var paginatedProducts = await productRepository.GetPaginatedResultAsync(
                 request.PageNumber,
                 request.PageSize,
                 predicate: product => product.CategoryId == request.Id, // Lọc theo BrandId
                 isTracking: false,
                 includeProperties: p => p.ProductDetails!
             );

            // Ánh xạ các Entity Product sang ProductDTO
            var productDTOs = new List<ProductDTO>();

            foreach (var product in paginatedProducts.Items)
            {
                // Truy vấn thông tin Category, Brand, Shop cho từng sản phẩm
                var brand = await brandRepository.FindByIdAsync(product.BrandId!.Value, true, cancellationToken);

                // Ánh xạ vào ProductDTO
                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Views = product.Views,
                    Price = product.Price,
                    Brand = brand != null ? new BrandDTO { Id = brand.Id, Name = brand.Name, Icon = brand.Icon } : null,
                    Image = product.Image,
                    InsertedAt = product.InsertedAt,
                    IsDeleted = product.IsDeleted,
                    ProductDetails = product.ProductDetails!.Select(pd => new Entities.ProductDetail
                    {
                        Id = pd.Id,
                        Color = pd.Color,
                        Size = pd.Size,
                        StockQuantity = pd.StockQuantity,
                        ProductId = pd.ProductId,
                    }).ToList()
                };

                productDTOs.Add(productDTO);
            }

            // Trả về kết quả dưới dạng Result<PaginatedResult<ProductDTO>>
            var result = new PaginatedResult<ProductDTO>(
                request.PageNumber,
                request.PageSize,
                paginatedProducts.TotalCount,
                productDTOs
            );

            return await Task.FromResult(result);
        }
    }
}