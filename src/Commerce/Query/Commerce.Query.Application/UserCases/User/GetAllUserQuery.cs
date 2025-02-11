using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Application.UserCases.User
{
    /// <summary>
    /// Request to get all user
    /// </summary>
    public class GetAllUserQuery : IRequest<Result<PaginatedResult<Entities.User>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;

        public GetAllUserQuery(int pageNumber)
        {
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all user request
    /// </summary>
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, Result<PaginatedResult<Entities.User>>>
    {
        private readonly IUserRepository userRepository;

        public GetAllUserQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<PaginatedResult<Entities.User>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 5;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.User, bool>> predicate = user => true;

            // Gọi phương thức GetPaginatedResultAsync từ repository để lấy dữ liệu phân trang
            var paginatedUsers = await userRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                predicate,
                isTracking: false,
                includeProperties: Array.Empty<Expression<Func<Entities.User, object>>>()
            );

            // Trả về kết quả dưới dạng Result<PaginatedResult<Entities.User>>
            var result = new PaginatedResult<Entities.User>(
                pageNumber,
                pageSize,
                paginatedUsers.TotalCount,
                paginatedUsers.Items
            );

            return await Task.FromResult(result);
        }
    }
}
