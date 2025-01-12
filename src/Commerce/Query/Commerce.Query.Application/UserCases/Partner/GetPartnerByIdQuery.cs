using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Partner;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Partner;

namespace Commerce.Query.Application.UserCases.Partner
{
    /// <summary>
    /// Request to get partner by id
    /// </summary>
    public record GetPartnerByIdQuery : IRequest<Result<Entities.Partner>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get partner by id request
    /// </summary>
    public class GetPartnerByIdQueryHandler : IRequestHandler<GetPartnerByIdQuery, Result<Entities.Partner>>
    {
        private readonly IPartnerRepository partnerRepository;

        /// <summary>
        /// Handler for get partner by id request
        /// </summary>
        public GetPartnerByIdQueryHandler(IPartnerRepository partnerRepository)
        {
            this.partnerRepository = partnerRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with partner data</returns>
        public async Task<Result<Entities.Partner>> Handle(GetPartnerByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            // Find partner without allow null return. If partner not found will throw NotFoundException
            var partner = await partnerRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            return partner!;
        }
    }
}
