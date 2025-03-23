using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.Abstractions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using Commerce.Query.Domain.Abstractions.Repositories.Ward;
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
        private readonly IWardRepository wardRepository;
        private readonly IDistrictRepository districtRepository;
        private readonly IProvinceRepository provinceRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IWardRepository wardRepository, IDistrictRepository districtRepository, IProvinceRepository provinceRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userRoleRepository = userRoleRepository;
            this.wardRepository = wardRepository;
            this.districtRepository = districtRepository;
            this.provinceRepository = provinceRepository;
        }

        public async Task<Result<PaginatedResult<UserDTO>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = request.PageSize;

            // Lấy danh sách User có phân trang
            var paginatedUsers = await userRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                isTracking: false,
                includeProperties: Array.Empty<Expression<Func<Entities.User, object>>>()
            );

            var userIds = paginatedUsers.Items.Select(u => u.Id).ToList();

            // Lấy danh sách UserRole và Role tương ứng
            var userRoles = userRoleRepository.FindAll(ur => userIds.Contains(ur.UserId!.Value)).ToList();
            var roleIds = userRoles.Select(ur => ur.RoleId).Distinct().ToList();
            var roles = roleRepository.FindAll(r => roleIds.Contains(r.Id)).ToList();

            // Lấy danh sách Ward
            var wardIds = paginatedUsers.Items.Where(u => u.WardId.HasValue).Select(u => u.WardId.Value).Distinct().ToList();
            var wards = wardRepository.FindAll(w => wardIds.Contains(w.Id)).ToList();

            // Lấy danh sách District từ danh sách Ward
            var districtIds = wards.Select(w => w.DistrictId).Distinct().ToList();
            var districts = districtRepository.FindAll(d => districtIds.Contains(d.Id)).ToList();

            // Lấy danh sách Province từ danh sách District
            var provinceIds = districts.Select(d => d.ProvinceId).Distinct().ToList();
            var provinces = provinceRepository.FindAll(p => provinceIds.Contains(p.Id)).ToList();

            // Map từ Entities.User sang UserDTO
            var userDTOs = paginatedUsers.Items.Select(user =>
            {
                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Address = user.Address,
                    Email = user.Email,
                    Tel = user.Tel,
                    WardId = user.WardId,
                    InsertedAt = user.InsertedAt,
                    IsDeleted = user.IsDeleted,
                    Roles = roles
                        .Where(r => userRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == r.Id))
                        .ToList(),
                    wards = wards.Where(w => w.Id == user.WardId).Select(w => new WardFullDTO
                    {
                        Id = w.Id,
                        Name = w.Name,
                        FullName = w.FullName,
                        DistrictId = w.DistrictId,
                        district = districts.Where(d => d.Id == w.DistrictId).Select(d => new DistrictDTO
                        {
                            Id = d.Id,
                            Name = d.Name,
                            FullName = d.FullName,
                            ProvinceId = d.ProvinceId,
                            province = provinces.Where(p => p.Id == d.ProvinceId).Select(p => new ProvinceDTO
                            {
                                Id = p.Id,
                                Name = p.Name,
                                FullName = p.FullName
                            }).FirstOrDefault()!
                        }).FirstOrDefault()!
                    }).FirstOrDefault()!
                };
                return userDTO;
            }).ToList();

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