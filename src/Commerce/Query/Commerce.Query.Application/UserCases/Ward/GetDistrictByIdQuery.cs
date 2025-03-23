using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Ward;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Ward;

namespace Commerce.Query.Application.DistrictCases.District
{
    /// <summary>
    /// Request to get district by id
    /// </summary>
    public record GetDistrictByIdQuery : IRequest<Result<List<Entities.District>>>
    {
        public int? ProvinceId { get; init; }
    }

    /// <summary>
    /// Handler for get district by id request
    /// </summary>
    public class GetDistrictByIdQueryHandler : IRequestHandler<GetDistrictByIdQuery, Result<List<Entities.District>>>
    {
        private readonly IDistrictRepository districtRepository;

        /// <summary>
        /// Handler for get district by id request
        /// </summary>
        public GetDistrictByIdQueryHandler(IDistrictRepository districtRepository)
        {
            this.districtRepository = districtRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with district data</returns>
        public async Task<Result<List<Entities.District>>> Handle(GetDistrictByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Find district without allow null return. If district not found will throw NotFoundException
            var district = districtRepository.FindAll(x => x.ProvinceId == request.ProvinceId!.Value).ToList();
            return district!;
        }
    }
}
