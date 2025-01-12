using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Category;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Category;

namespace Commerce.Query.Application.UserCases.Category
{
    /// <summary>
    /// Request to get category by id
    /// </summary>
    public record GetCategoryByIdQuery : IRequest<Result<Entities.Category>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get category by id request
    /// </summary>
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<Entities.Category>>
    {
        private readonly ICategoryRepository categoryRepository;

        /// <summary>
        /// Handler for get category by id request
        /// </summary>
        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with category data</returns>
        public async Task<Result<Entities.Category>> Handle(GetCategoryByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            // Find category without allow null return. If category not found will throw NotFoundException
            var category = await categoryRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            return category!;
        }
    }
}
