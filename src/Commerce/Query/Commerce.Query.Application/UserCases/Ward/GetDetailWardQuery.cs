using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Ward;
using MediatR;

namespace Commerce.Query.Application.WardCases.Ward
{
    /// <summary>
    /// Request to get ward by id
    /// </summary>
    public record GetDetailWardQuery : IRequest<Result<WardFullDTO>>
    {
        public int Id { get; init; }
    }

    /// <summary>
    /// Handler for get ward by id request
    /// </summary>
    public class GetDetailWardQueryHandler : IRequestHandler<GetDetailWardQuery, Result<WardFullDTO>>
    {
        private readonly IWardRepository wardRepository;
        private readonly IDistrictRepository districtRepository;
        private readonly IProvinceRepository provinceRepository;

        /// <summary>
        /// Handler for get ward by id request
        /// </summary>
        public GetDetailWardQueryHandler(IWardRepository wardRepository, IDistrictRepository districtRepository, IProvinceRepository provinceRepository)
        {
            this.wardRepository = wardRepository;
            this.districtRepository = districtRepository;
            this.provinceRepository = provinceRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with ward data</returns>
        public async Task<Result<WardFullDTO>> Handle(GetDetailWardQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Find ward without allow null return. If ward not found will throw NotFoundException
            var ward = await wardRepository.FindByIdAsync(request.Id, false, cancellationToken);
            var district = await districtRepository.FindByIdAsync(ward.DistrictId, false, cancellationToken);
            var province = await provinceRepository.FindByIdAsync(district.ProvinceId, false, cancellationToken);

            var wardDto = ward.MapTo<WardFullDTO>();
            wardDto.district = district.MapTo<DistrictDTO>();
            wardDto.district.province = province.MapTo<ProvinceDTO>();

            return wardDto!;
        }
    }
}
