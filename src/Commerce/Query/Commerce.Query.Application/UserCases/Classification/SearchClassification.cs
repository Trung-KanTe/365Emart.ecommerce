using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Category;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.Category;

namespace Commerce.Query.Application.ClassificationCases.Classification
{
    /// <summary>
    /// Request to get all classification
    /// </summary>
    public class SearchClassificationQuery : IRequest<Result<PaginatedResult<Entities.Classification>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public string Name { get; set; }

        public SearchClassificationQuery(string Name, int pageNumber)
        {
            this.Name = Name;
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all classification request
    /// </summary>
    public class SearchClassificationQueryHandler : IRequestHandler<SearchClassificationQuery, Result<PaginatedResult<Entities.Classification>>>
    {
        private readonly IClassificationRepository classificationRepository;

        public SearchClassificationQueryHandler(IClassificationRepository classificationRepository)
        {
            this.classificationRepository = classificationRepository;
        }

        public async Task<Result<PaginatedResult<Entities.Classification>>> Handle(SearchClassificationQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 5;
            string searchTerm = request.Name?.Trim().ToLower()!;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.Classification, bool>> predicate = p => string.IsNullOrEmpty(searchTerm) || p.Name.ToLower().Contains(searchTerm);

            // Gọi phương thức GetPaginatedResultAsync từ repository để lấy dữ liệu phân trang
            var paginatedClassifications = await classificationRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                predicate,
                isTracking: false,
                includeProperties: Array.Empty<Expression<Func<Entities.Classification, object>>>()
            );

            // Trả về kết quả dưới dạng Result<PaginatedResult<Entities.Classification>>
            var result = new PaginatedResult<Entities.Classification>(
                pageNumber,
                pageSize,
                paginatedClassifications.TotalCount,
                paginatedClassifications.Items
            );

            return await Task.FromResult(result);
        }
    }
}
