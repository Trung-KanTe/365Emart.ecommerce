using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Brand;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.Brand;

namespace Commerce.Query.Application.BrandCases.Brand
{
    /// <summary>
    /// Request to get all brand
    /// </summary>
    public class GetAllBrandQuery : IRequest<Result<PaginatedResult<Entities.Brand>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;

        public GetAllBrandQuery(int pageNumber)
        {
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all brand request
    /// </summary>
    public class GetAllBrandQueryHandler : IRequestHandler<GetAllBrandQuery, Result<PaginatedResult<Entities.Brand>>>
    {
        private readonly IBrandRepository brandRepository;

        public GetAllBrandQueryHandler(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }

        public async Task<Result<PaginatedResult<Entities.Brand>>> Handle(GetAllBrandQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 5;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.Brand, bool>> predicate = brand => true;

            // Gọi phương thức GetPaginatedResultAsync từ repository để lấy dữ liệu phân trang
            var paginatedBrands = await brandRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                predicate,
                isTracking: false,
                includeProperties: Array.Empty<Expression<Func<Entities.Brand, object>>>()
            );

            // Trả về kết quả dưới dạng Result<PaginatedResult<Entities.Brand>>
            var result = new PaginatedResult<Entities.Brand>(
                pageNumber,
                pageSize,
                paginatedBrands.TotalCount,
                paginatedBrands.Items
            );

            return await Task.FromResult(result);
        }
    }
}
