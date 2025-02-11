using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.Shop;

namespace Commerce.Query.Application.UserCases.Shop
{
    /// <summary>
    /// Request to get all shop
    /// </summary>
    public class GetAllShopPagingQuery : IRequest<Result<PaginatedResult<Entities.Shop>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;

        public GetAllShopPagingQuery(int pageNumber)
        {
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all shop request
    /// </summary>
    public class GetAllShopPagingQueryHandler : IRequestHandler<GetAllShopPagingQuery, Result<PaginatedResult<Entities.Shop>>>
    {
        private readonly IShopRepository shopRepository;

        public GetAllShopPagingQueryHandler(IShopRepository shopRepository)
        {
            this.shopRepository = shopRepository;
        }

        public async Task<Result<PaginatedResult<Entities.Shop>>> Handle(GetAllShopPagingQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 5;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.Shop, bool>> predicate = shop => true;

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
