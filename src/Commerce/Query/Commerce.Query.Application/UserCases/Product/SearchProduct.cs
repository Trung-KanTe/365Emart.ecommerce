using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Brand;
using Commerce.Query.Domain.Abstractions.Repositories.Category;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Product
{
    /// <summary>
    /// Request to get all product
    /// </summary>
    public class SearchProductQuery : IRequest<Result<PaginatedResult<ProductDTO>>>
    {
        public Guid? Id { get; init; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public string Name { get; set; }

        public SearchProductQuery(Guid? id, string Name, int pageNumber)
        {
            Id = id;
            this.Name = Name;
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all product request
    /// </summary>
    public class SearchProductQueryHandler : IRequestHandler<SearchProductQuery, Result<PaginatedResult<ProductDTO>>>
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IBrandRepository brandRepository;
        private readonly IShopRepository shopRepository;

        public SearchProductQueryHandler(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IBrandRepository brandRepository,
            IShopRepository shopRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.brandRepository = brandRepository;
            this.shopRepository = shopRepository;
        }

        public async Task<Result<PaginatedResult<ProductDTO>>> Handle(SearchProductQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 10;
            string searchTerm = request.Name?.Trim().ToLower()!;
            var shops = await shopRepository.FindSingleAsync(x => x.UserId == request.Id, true, cancellationToken);

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.Product, bool>> predicate = p => string.IsNullOrEmpty(searchTerm) || p.Name.ToLower().Contains(searchTerm) && p.ShopId == shops.Id;

            // Gọi phương thức GetPaginatedResultAsync từ repository để lấy dữ liệu phân trang
            var paginatedProducts = await productRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                predicate,
                isTracking: false,
                includeProperties: p => p.ProductDetails!  // Include ProductDetails
            );

            // Ánh xạ các Entity Product sang ProductDTO
            var productDTOs = new List<ProductDTO>();

            foreach (var product in paginatedProducts.Items)
            {
                // Truy vấn thông tin Category, Brand, Shop cho từng sản phẩm
                var category = await categoryRepository.FindByIdAsync(product.CategoryId!.Value, true, cancellationToken);
                var brand = await brandRepository.FindByIdAsync(product.BrandId!.Value, true, cancellationToken);
                var shop = await shopRepository.FindByIdAsync(product.ShopId!.Value, true, cancellationToken);

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
                pageNumber,
                pageSize,
                paginatedProducts.TotalCount,
                productDTOs
            );

            return await Task.FromResult(result);
        }
    }
}
