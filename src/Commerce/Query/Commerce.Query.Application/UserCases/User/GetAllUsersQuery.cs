using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.Abstractions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Application.UserCases.User
{
    /// <summary>
    /// Request to get all user with pagination
    /// </summary>
    public class GetAllUsersQuery : IRequest<Result<PaginatedResult<UserDTO>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;

        public GetAllUsersQuery(int pageNumber, int pageSize = 5)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    /// <summary>
    /// Handler for get all users request with localization info
    /// </summary>
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<PaginatedResult<UserDTO>>>
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IWebWardService webWardService;

        public GetAllUsersQueryHandler(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IWebWardService webWardService)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userRoleRepository = userRoleRepository;
            this.webWardService = webWardService;
        }

        public async Task<Result<PaginatedResult<UserDTO>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = request.PageSize;

            // Predicate mặc định (có thể thêm điều kiện lọc nếu cần)
            Expression<Func<UserDTO, bool>> predicate = user => true;

            var paginatedUsers = await userRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                isTracking: false,
                includeProperties: Array.Empty<Expression<Func<Entities.User, object>>>()
            );

            var userIds = paginatedUsers.Items.Select(c => c.Id).ToList();
            var userRoles = userRoleRepository
                .FindAll(cc => userIds.Contains(cc.UserId!.Value))
                .ToList();

            // Lấy các Classification từ bảng Classification tương ứng với ClassificationCategory
            var roleIds = userRoles.Select(cc => cc.RoleId).Distinct().ToList();
            var roles = roleRepository
                .FindAll(c => roleIds.Contains(c.Id))
                .ToList();
           
            // Map từ Entities.User sang UserDTO
            var userDTOs = paginatedUsers.Items.Select(category => 
            {
                var categoryDTO = new UserDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Address = category.Address,
                    Email = category.Email,
                    Tel = category.Tel,
                    InsertedAt = category.InsertedAt,
                    IsDeleted = category.IsDeleted,
                    Roles = roles
                        .Where(c => userRoles.Any(cc => cc.UserId == category.Id && cc.RoleId == c.Id))
                        .ToList()
                };
            return categoryDTO;
             }).ToList();

            // Lấy danh sách wardId duy nhất
            var wardIds = userDTOs
                .Where(user => user.WardId.HasValue)
                .Select(user => user.WardId.Value)
                .Distinct()
                .ToArray();

            // Gọi service để lấy thông tin địa phương
            var localizations = await webWardService.GetLocalFullsByWardIds(wardIds);

            // Map dữ liệu LocalizationFullDTO vào từng user
            foreach (var user in userDTOs)
            {
                var localization = localizations.FirstOrDefault(l => l.WardId == user.WardId);
                user.LocalizationFullDTO = localization;
            }

            // Trả về kết quả dưới dạng Result<PaginatedResult<UserDTO>>
            var result = new PaginatedResult<UserDTO>(
                pageNumber,
                pageSize,
                paginatedUsers.TotalCount,
                userDTOs
            );

            return result;
        }
    }
}