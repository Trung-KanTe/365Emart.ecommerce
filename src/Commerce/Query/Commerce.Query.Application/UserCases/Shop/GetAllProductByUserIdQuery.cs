using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Brand;
using Commerce.Query.Domain.Abstractions.Repositories.Category;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Shop
{
    /// <summary>
    /// Request to get brand by id
    /// </summary>
    public record GetAllProductByUserIdQuery : IRequest<Result<PaginatedResult<ProductDTO>>>
    {
        public Guid? Id { get; init; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllProductByUserIdQuery(Guid? id, int pageNumber, int pageSize)
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
    public class GetAllProductByUserIdQueryHandler : IRequestHandler<GetAllProductByUserIdQuery, Result<PaginatedResult<ProductDTO>>>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IBrandRepository brandRepository;
        private readonly IShopRepository shopRepository;

        public GetAllProductByUserIdQueryHandler(IProductRepository productRepository, ICategoryRepository categoryRepository,
            IBrandRepository brandRepository, IProductDetailRepository productDetailRepository, IShopRepository shopRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.brandRepository = brandRepository;
            this.productDetailRepository = productDetailRepository;
            this.shopRepository = shopRepository;
        }

        public async Task<Result<PaginatedResult<ProductDTO>>> Handle(GetAllProductByUserIdQuery request, CancellationToken cancellationToken)
        {
            var shop = await shopRepository.FindSingleAsync(x => x.UserId == request.Id, true, cancellationToken);

            // Lấy danh sách sản phẩm theo ShopId và phân trang
            var paginatedProducts = await productRepository.GetPaginatedResultAsync(
                request.PageNumber,
                request.PageSize,
                predicate: product => product.ShopId == shop.Id, // Lọc theo ShopId
                isTracking: false,
                includeProperties: p => p.ProductDetails!
            );

            var productDTOs = new List<ProductDTO>();

            foreach (var product in paginatedProducts.Items)
            {
                // Truy vấn thông tin Category, Brand, Shop cho từng sản phẩm
                var category = await categoryRepository.FindByIdAsync(product.CategoryId!.Value, true, cancellationToken);
                var brand = await brandRepository.FindByIdAsync(product.BrandId!.Value, true, cancellationToken);

                // Ánh xạ vào ProductDTO
                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Views = product.Views,
                    Price = product.Price,
                    Category = category != null ? new CatDTO { Id = category.Id, Name = category.Name } : null,
                    Brand = brand != null ? new BrandDTO { Id = brand.Id, Name = brand.Name } : null,
                    Shop = shop != null ? new ShopDTO { Id = shop.Id, Name = shop.Name } : null,
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