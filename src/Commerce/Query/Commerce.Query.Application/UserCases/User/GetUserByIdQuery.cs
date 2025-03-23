using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using Commerce.Query.Domain.Abstractions.Repositories.Ward;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Application.UserCases.User
{
    /// <summary>
    /// Request to get user by id
    /// </summary>
    public record GetUserByIdQuery : IRequest<Result<UserDTO>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get user by id request
    /// </summary>
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDTO>>
    {
        private readonly IUserRepository userRepository;
        private readonly IWardRepository wardRepository;
        private readonly IDistrictRepository districtRepository;
        private readonly IProvinceRepository provinceRepository;

        /// <summary>
        /// Handler for get user by id request
        /// </summary>
        public GetUserByIdQueryHandler(IUserRepository userRepository, IWardRepository wardRepository, IDistrictRepository districtRepository, IProvinceRepository provinceRepository)
        {
            this.userRepository = userRepository;
            this.wardRepository = wardRepository;
            this.districtRepository = districtRepository;
            this.provinceRepository = provinceRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with user data</returns>
        public async Task<Result<UserDTO>> Handle(GetUserByIdQuery request,
                                                 CancellationToken cancellationToken)
        {

            // Find user without allow null return. If user not found will throw NotFoundException
            var user = await userRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);

            var ward = await wardRepository.FindByIdAsync(user.WardId!.Value, false, cancellationToken);
            var district = await districtRepository.FindByIdAsync(ward.DistrictId, false, cancellationToken);
            var province = await provinceRepository.FindByIdAsync(district.ProvinceId, false, cancellationToken);

            var userDTO = user.MapTo<UserDTO>();
            var wardDto = ward.MapTo<WardFullDTO>();
            wardDto.district = district.MapTo<DistrictDTO>();
            wardDto.district.province = province.MapTo<ProvinceDTO>();

            userDTO.wards = wardDto;
            return userDTO!;
        }
    }
}
