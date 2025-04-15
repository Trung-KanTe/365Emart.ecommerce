using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.Shop;

namespace Commerce.Query.Application.ShopCases.Shop
{
    /// <summary>
    /// Request to get all shop
    /// </summary>
    public class SearchShopQuery : IRequest<Result<PaginatedResult<Entities.Shop>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public string Name { get; set; }

        public SearchShopQuery(string Name, int pageNumber)
        {
            this.Name = Name;
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all shop request
    /// </summary>
    public class SearchShopQueryHandler : IRequestHandler<SearchShopQuery, Result<PaginatedResult<Entities.Shop>>>
    {
        private readonly IShopRepository shopRepository;

        public SearchShopQueryHandler(IShopRepository shopRepository)
        {
            this.shopRepository = shopRepository;
        }

        public async Task<Result<PaginatedResult<Entities.Shop>>> Handle(SearchShopQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 5;
            string searchTerm = request.Name?.Trim().ToLower()!;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.Shop, bool>> predicate = p => string.IsNullOrEmpty(searchTerm) || p.Name.ToLower().Contains(searchTerm);

            // Gọi phương thức GetPaginatedResultAsync từ repository để lấy dữ liệu phân trang
            var paginatedShops = await shopRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                predicate,
                isTracking: false,
                includeProperties: Array.Empty<Expression<Func<Entities.Shop, object>>>()
            );

            // Trả về kết quả dưới dạng Result<PaginatedResult<Entities.Shop>>
            var result = new PaginatedResult<Entities.Shop>(
                pageNumber,
                pageSize,
                paginatedShops.TotalCount,
                paginatedShops.Items
            );

            return await Task.FromResult(result);
        }
    }
}
