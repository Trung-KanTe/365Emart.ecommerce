using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Partner;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.Partner;

namespace Commerce.Query.Application.PartnerCases.Partner
{
    /// <summary>
    /// Request to get all partner
    /// </summary>
    public class SearchPartnerQuery : IRequest<Result<PaginatedResult<Entities.Partner>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public string Name { get; set; }

        public SearchPartnerQuery(string Name, int pageNumber)
        {
            this.Name = Name;
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all partner request
    /// </summary>
    public class SearchPartnerQueryHandler : IRequestHandler<SearchPartnerQuery, Result<PaginatedResult<Entities.Partner>>>
    {
        private readonly IPartnerRepository partnerRepository;

        public SearchPartnerQueryHandler(IPartnerRepository partnerRepository)
        {
            this.partnerRepository = partnerRepository;
        }

        public async Task<Result<PaginatedResult<Entities.Partner>>> Handle(SearchPartnerQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 5;
            string searchTerm = request.Name?.Trim().ToLower()!;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.Partner, bool>> predicate = p => string.IsNullOrEmpty(searchTerm) || p.Name.ToLower().Contains(searchTerm);

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
