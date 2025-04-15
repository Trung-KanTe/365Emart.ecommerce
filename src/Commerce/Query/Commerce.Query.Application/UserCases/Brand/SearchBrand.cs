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
    public class SearchBrandQuery : IRequest<Result<PaginatedResult<Entities.Brand>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public string Name { get; set; }

        public SearchBrandQuery(string Name, int pageNumber)
        {
            this.Name = Name;
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all brand request
    /// </summary>
    public class SearchBrandQueryHandler : IRequestHandler<SearchBrandQuery, Result<PaginatedResult<Entities.Brand>>>
    {
        private readonly IBrandRepository brandRepository;

        public SearchBrandQueryHandler(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }

        public async Task<Result<PaginatedResult<Entities.Brand>>> Handle(SearchBrandQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 5;
            string searchTerm = request.Name?.Trim().ToLower()!;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.Brand, bool>> predicate = p => string.IsNullOrEmpty(searchTerm) || p.Name.ToLower().Contains(searchTerm);

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
