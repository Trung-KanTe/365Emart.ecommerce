using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Category;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Category;

namespace Commerce.Query.Application.UserCases.Classification
{
    /// <summary>
    /// Request to get classification by id
    /// </summary>
    public record GetClassificationByIdQuery : IRequest<Result<Entities.Classification>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get classification by id request
    /// </summary>
    public class GetClassificationByIdQueryHandler : IRequestHandler<GetClassificationByIdQuery, Result<Entities.Classification>>
    {
        private readonly IClassificationRepository classificationRepository;

        /// <summary>
        /// Handler for get classification by id request
        /// </summary>
        public GetClassificationByIdQueryHandler(IClassificationRepository classificationRepository)
        {
            this.classificationRepository = classificationRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with classification data</returns>
        public async Task<Result<Entities.Classification>> Handle(GetClassificationByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            // Find classification without allow null return. If classification not found will throw NotFoundException
            var classification = await classificationRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            return classification!;
        }
    }
}
