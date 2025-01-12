using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Promotion;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Promotion;

namespace Commerce.Query.Application.UserCases.Promotion
{
    /// <summary>
    /// Request to get promotion by id
    /// </summary>
    public record GetPromotionByIdQuery : IRequest<Result<Entities.Promotion>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get promotion by id request
    /// </summary>
    public class GetPromotionByIdQueryHandler : IRequestHandler<GetPromotionByIdQuery, Result<Entities.Promotion>>
    {
        private readonly IPromotionRepository promotionRepository;

        /// <summary>
        /// Handler for get promotion by id request
        /// </summary>
        public GetPromotionByIdQueryHandler(IPromotionRepository promotionRepository)
        {
            this.promotionRepository = promotionRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with promotion data</returns>
        public async Task<Result<Entities.Promotion>> Handle(GetPromotionByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            // Find promotion without allow null return. If promotion not found will throw NotFoundException
            var promotion = await promotionRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            return promotion!;
        }
    }
}
