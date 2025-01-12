using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Product
{
    /// <summary>
    /// Request to get productReview by id
    /// </summary>
    public record GetProductReviewByIdQuery : IRequest<Result<Entities.ProductReview>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get productReview by id request
    /// </summary>
    public class GetProductReviewByIdQueryHandler : IRequestHandler<GetProductReviewByIdQuery, Result<Entities.ProductReview>>
    {
        private readonly IProductReviewRepository productReviewRepository;

        /// <summary>
        /// Handler for get productReview by id request
        /// </summary>
        public GetProductReviewByIdQueryHandler(IProductReviewRepository productReviewRepository)
        {
            this.productReviewRepository = productReviewRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with productReview data</returns>
        public async Task<Result<Entities.ProductReview>> Handle(GetProductReviewByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            // Find productReview without allow null return. If productReview not found will throw NotFoundException
            var productReview = await productReviewRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            return productReview!;
        }
    }
}
