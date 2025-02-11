using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Partner;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.Partner;

namespace Commerce.Query.Application.UserCases.Partner
{
    /// <summary>
    /// Request to get all partner
    /// </summary>
    public class GetAllPartnerPagingQuery : IRequest<Result<PaginatedResult<Entities.Partner>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;

        public GetAllPartnerPagingQuery(int pageNumber)
        {
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all partner request
    /// </summary>
    public class GetAllPartnerPagingQueryHandler : IRequestHandler<GetAllPartnerPagingQuery, Result<PaginatedResult<Entities.Partner>>>
    {
        private readonly IPartnerRepository partnerRepository;

        public GetAllPartnerPagingQueryHandler(IPartnerRepository partnerRepository)
        {
            this.partnerRepository = partnerRepository;
        }

        public async Task<Result<PaginatedResult<Entities.Partner>>> Handle(GetAllPartnerPagingQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 5;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.Partner, bool>> predicate = partner => true;

            // Gọi phương thức GetPaginatedResultAsync từ repository để lấy dữ liệu phân trang
            var paginatedPartners = await partnerRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                predicate,
                isTracking: false,
                includeProperties: Array.Empty<Expression<Func<Entities.Partner, object>>>()
            );

            // Trả về kết quả dưới dạng Result<PaginatedResult<Entities.Partner>>
            var result = new PaginatedResult<Entities.Partner>(
                pageNumber,
                pageSize,
                paginatedPartners.TotalCount,
                paginatedPartners.Items
            );

            return await Task.FromResult(result);
        }
    }
}
