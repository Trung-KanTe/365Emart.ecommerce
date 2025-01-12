using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Brand;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Brand;

namespace Commerce.Query.Application.UserCases.Brand
{
    /// <summary>
    /// Request to get brand by id
    /// </summary>
    public record GetBrandByIdQuery : IRequest<Result<Entities.Brand>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get brand by id request
    /// </summary>
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, Result<Entities.Brand>>
    {
        private readonly IBrandRepository brandRepository;

        /// <summary>
        /// Handler for get brand by id request
        /// </summary>
        public GetBrandByIdQueryHandler(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with brand data</returns>
        public async Task<Result<Entities.Brand>> Handle(GetBrandByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();
           
            // Find brand without allow null return. If brand not found will throw NotFoundException
            var brand = await brandRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            return brand!;
        }
    }
}
