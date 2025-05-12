using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
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
    public class SearchAllProductQuery : IRequest<Result<SearchAll>>
    {
        public string Name { get; set; }

        public SearchAllProductQuery(string Name)
        {
            this.Name = Name;
        }
    }

    /// <summary>
    /// Handler for get all product request
    /// </summary>
    public class SearchAllProductQueryHandler : IRequestHandler<SearchAllProductQuery, Result<SearchAll>>
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IBrandRepository brandRepository;
        private readonly IShopRepository shopRepository;
        private readonly IProductReviewRepository productReviewRepository;

        public SearchAllProductQueryHandler(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IBrandRepository brandRepository,
            IShopRepository shopRepository,
            IProductReviewRepository productReviewRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.brandRepository = brandRepository;
            this.shopRepository = shopRepository;
            this.productReviewRepository = productReviewRepository;
        }

        public async Task<Result<SearchAll>> Handle(SearchAllProductQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = 1;
            int pageSize = 20;
            string searchTerm = request.Name?.Trim().ToLower()!;

            Expression<Func<Entities.Product, bool>> predicate = p =>
                string.IsNullOrEmpty(searchTerm) || p.Name.ToLower().Contains(searchTerm);

            var allMatchedProducts = (productRepository.FindAll(
                includeProperties: x => x.ProductDetails!
            )).Where(predicate.Compile()).ToList(); 

            var allReviews = productReviewRepository.FindAll();
            var reviewCounts = allReviews
                .GroupBy(r => r.ProductId)!
                .ToDictionary(g => g.Key, g => g.Count());

            var productWithReviewCounts = allMatchedProducts
                .Select(p => new
                {
                    Product = p,
                    ReviewCount = reviewCounts.ContainsKey(p.Id) ? reviewCounts[p.Id] : 0
                })
                .ToList();

            var sortedProducts = productWithReviewCounts
                .OrderByDescending(x => x.ReviewCount)
                .ThenBy(x => Guid.NewGuid())
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => x.Product)
                .ToList();

            var productDTOs = new List<ProductDTO>();

            foreach (var product in sortedProducts)
            {
                var category = await categoryRepository.FindByIdAsync(product.CategoryId!.Value, true, cancellationToken);
                var brand = await brandRepository.FindByIdAsync(product.BrandId!.Value, true, cancellationToken);
                var shop = await shopRepository.FindByIdAsync(product.ShopId!.Value, true, cancellationToken);

                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Views = product.Views,
                    Price = product.Price,
                    Category = category != null ? new CatDTO { Id = category.Id, Name = category.Name } : null,
                    Brand = brand != null ? new BrandDTO { Id = brand.Id, Name = brand.Name, Icon = brand.Icon } : null,
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

            // Tìm Brand nếu trùng khớp tên chính xác
            var matchedBrand = await brandRepository.FindSingleAsync(b => b.Name.ToLower() == searchTerm, true, cancellationToken);
            BrandDTO? brandDTO = matchedBrand.MapTo<BrandDTO>();

            // Tìm Shop nếu trùng khớp tên chính xác
            var matchedShop = await shopRepository.FindSingleAsync(s => s.Name.ToLower() == searchTerm, true, cancellationToken);
            ShopDTO? shopDTO = matchedShop.MapTo<ShopDTO>();

            var result = new SearchAll
            {
                Products = productDTOs,
                brand = brandDTO,
                shop = shopDTO
            };

            return result;
        }
    }
}
