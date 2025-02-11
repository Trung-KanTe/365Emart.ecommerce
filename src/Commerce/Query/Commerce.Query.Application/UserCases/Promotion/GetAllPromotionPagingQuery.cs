using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Promotion;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.Promotion;

namespace Commerce.Query.Application.UserCases.Promotion
{
    /// <summary>
    /// Request to get all promotion
    /// </summary>
    public class GetAllPromotionPagingQuery : IRequest<Result<PaginatedResult<Entities.Promotion>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;

        public GetAllPromotionPagingQuery(int pageNumber)
        {
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all promotion request
    /// </summary>
    public class GetAllPromotionPagingQueryHandler : IRequestHandler<GetAllPromotionPagingQuery, Result<PaginatedResult<Entities.Promotion>>>
    {
        private readonly IPromotionRepository promotionRepository;

        public GetAllPromotionPagingQueryHandler(IPromotionRepository promotionRepository)
        {
            this.promotionRepository = promotionRepository;
        }

        public async Task<Result<PaginatedResult<Entities.Promotion>>> Handle(GetAllPromotionPagingQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 5;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.Promotion, bool>> predicate = promotion => true;

            // Gọi phương thức GetPaginatedResultAsync từ repository để lấy dữ liệu phân trang
            var paginatedPromotions = await promotionRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                predicate,
                isTracking: false,
                includeProperties: Array.Empty<Expression<Func<Entities.Promotion, object>>>()
            );

            // Trả về kết quả dưới dạng Result<PaginatedResult<Entities.Promotion>>
            var result = new PaginatedResult<Entities.Promotion>(
                pageNumber,
                pageSize,
                paginatedPromotions.TotalCount,
                paginatedPromotions.Items
            );

            return await Task.FromResult(result);
        }
    }
}