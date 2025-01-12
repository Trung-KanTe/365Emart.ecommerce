using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Product
{
    /// <summary>
    /// Request to get all productReview
    /// </summary>
    public class GetAllProductReviewQuery : IRequest<Result<List<Entities.ProductReview>>>
    {
    }

    /// <summary>
    /// Handler for get all productReview request
    /// </summary>
    public class GetAllProductReviewQueryHandler : IRequestHandler<GetAllProductReviewQuery, Result<List<Entities.ProductReview>>>
    {
        private readonly IProductReviewRepository productReviewRepository;

        /// <summary>
        /// Handler for get all productReview request
        /// </summary>
        public GetAllProductReviewQueryHandler(IProductReviewRepository productReviewRepository)
        {
            this.productReviewRepository = productReviewRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list productReview as data</returns>
        public async Task<Result<List<Entities.ProductReview>>> Handle(GetAllProductReviewQuery request,
                                                       CancellationToken cancellationToken)
        {
            var productReviews = productReviewRepository.FindAll().ToList();
            return await Task.FromResult(productReviews);
        }
    }
}
