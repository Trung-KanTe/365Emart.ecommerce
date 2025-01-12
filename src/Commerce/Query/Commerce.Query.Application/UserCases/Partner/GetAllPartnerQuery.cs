using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Partner;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Partner;

namespace Commerce.Query.Application.UserCases.Partner
{
    /// <summary>
    /// Request to get all partner
    /// </summary>
    public class GetAllPartnerQuery : IRequest<Result<List<Entities.Partner>>>
    {
    }

    /// <summary>
    /// Handler for get all partner request
    /// </summary>
    public class GetAllPartnerQueryHandler : IRequestHandler<GetAllPartnerQuery, Result<List<Entities.Partner>>>
    {
        private readonly IPartnerRepository partnerRepository;

        /// <summary>
        /// Handler for get all partner request
        /// </summary>
        public GetAllPartnerQueryHandler(IPartnerRepository partnerRepository)
        {
            this.partnerRepository = partnerRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list partner as data</returns>
        public async Task<Result<List<Entities.Partner>>> Handle(GetAllPartnerQuery request,
                                                       CancellationToken cancellationToken)
        {
            var partners = partnerRepository.FindAll().ToList();
            return await Task.FromResult(partners);
        }
    }
}
