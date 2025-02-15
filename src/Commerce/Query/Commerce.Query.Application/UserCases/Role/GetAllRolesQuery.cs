using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Application.UserCases.Role
{
    /// <summary>
    /// Request to get all role
    /// </summary>
    public class GetAllRolesQuery : IRequest<Result<PaginatedResult<Entities.Role>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;

        public GetAllRolesQuery(int pageNumber)
        {
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all role request
    /// </summary>
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, Result<PaginatedResult<Entities.Role>>>
    {
        private readonly IRoleRepository roleRepository;

        public GetAllRolesQueryHandler(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<Result<PaginatedResult<Entities.Role>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 5;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.Role, bool>> predicate = role => true;

            // Gọi phương thức GetPaginatedResultAsync từ repository để lấy dữ liệu phân trang
            var paginatedRoles = await roleRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                predicate,
                isTracking: false,
                includeProperties: Array.Empty<Expression<Func<Entities.Role, object>>>()
            );

            // Trả về kết quả dưới dạng Result<PaginatedResult<Entities.Role>>
            var result = new PaginatedResult<Entities.Role>(
                pageNumber,
                pageSize,
                paginatedRoles.TotalCount,
                paginatedRoles.Items
            );

            return await Task.FromResult(result);
        }
    }
}
