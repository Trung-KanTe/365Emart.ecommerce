using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using MediatR;
using System.Linq;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Product
{
    /// <summary>
    /// Request to get all productReview
    /// </summary>
    public class GetAllProductReviewQuery : IRequest<Result<List<ProductReviewDTO>>>
    {
        public Guid? ProductId { get; init; }

    }

    /// <summary>
    /// Handler for get all productReview request
    /// </summary>
    public class GetAllProductReviewQueryHandler : IRequestHandler<GetAllProductReviewQuery, Result<List<ProductReviewDTO>>>
    {
        private readonly IProductReviewRepository productReviewRepository;
        private readonly IUserRepository userRepository;
        private readonly IProductReviewReplyRepository productReviewReplyRepository;

        /// <summary>
        /// Handler for get all productReview request
        /// </summary>
        public GetAllProductReviewQueryHandler(IProductReviewRepository productReviewRepository, IProductReviewReplyRepository productReviewReplyRepository, IUserRepository userRepository)
        {
            this.productReviewRepository = productReviewRepository;
            this.userRepository = userRepository;
            this.productReviewReplyRepository = productReviewReplyRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list productReview as data</returns>
        public async Task<Result<List<ProductReviewDTO>>> Handle(GetAllProductReviewQuery request,
                                                         CancellationToken cancellationToken)
        {
            // Lấy tất cả review theo ProductId
            var productReviews = productReviewRepository.FindAll(x => x.ProductId == request.ProductId).ToList();

            // Lấy danh sách UserId từ review
            var userIds = productReviews.Select(x => x.UserId).Distinct().ToList();
            var users = userRepository.FindAll(u => userIds.Contains(u.Id))
                                      .ToDictionary(u => u.Id, u => u);

            // Lấy tất cả reply theo danh sách reviewId
            var reviewIds = productReviews.Select(x => x.Id).ToList();
            var replies = productReviewReplyRepository.FindAll(r => reviewIds.Contains(r.ReviewId)).ToList();

            // Map từng review với user và reply
            var productReviewDTOs = productReviews.Select(review =>
            {
                var reply = replies.FirstOrDefault(r => r.ReviewId == review.Id);
                return new ProductReviewDTO
                {
                    Id = review.Id,
                    ProductId = review.ProductId,
                    UserId = review.UserId,
                    Rating = review.Rating,
                    Image = review.Image,
                    Comment = review.Comment,
                    InsertedAt = review.InsertedAt,
                    User = review.UserId.HasValue && users.TryGetValue(review.UserId.Value, out var user)
                            ? new UserDTO { Id = user.Id, Name = user.Name }
                            : null!,
                    ProductReviewReply = reply
                };
            }).ToList();

            return productReviewDTOs;
        }

    }
}
