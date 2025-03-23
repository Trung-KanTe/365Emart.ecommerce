using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Ward;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Ward;

namespace Commerce.Query.Application.WardCases.Ward
{
    /// <summary>
    /// Request to get ward by id
    /// </summary>
    public record GetWardByIdQuery : IRequest<Result<List<Entities.Ward>>>
    {
        public int? DistrictId { get; init; }
    }

    /// <summary>
    /// Handler for get ward by id request
    /// </summary>
    public class GetWardByIdQueryHandler : IRequestHandler<GetWardByIdQuery, Result<List<Entities.Ward>>>
    {
        private readonly IWardRepository wardRepository;

        /// <summary>
        /// Handler for get ward by id request
        /// </summary>
        public GetWardByIdQueryHandler(IWardRepository wardRepository)
        {
            this.wardRepository = wardRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with ward data</returns>
        public async Task<Result<List<Entities.Ward>>> Handle(GetWardByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Find ward without allow null return. If ward not found will throw NotFoundException
            var ward = wardRepository.FindAll(x => x.DistrictId == request.DistrictId!.Value).ToList();
            return ward!;
        }
    }
}
