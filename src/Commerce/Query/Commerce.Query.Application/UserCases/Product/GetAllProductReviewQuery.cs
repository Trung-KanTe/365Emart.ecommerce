using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using MediatR;
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

        /// <summary>
        /// Handler for get all productReview request
        /// </summary>
        public GetAllProductReviewQueryHandler(IProductReviewRepository productReviewRepository, IUserRepository userRepository)
        {
            this.productReviewRepository = productReviewRepository;
            this.userRepository = userRepository;
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
            // Lấy danh sách review theo ProductId
            var productReviews = productReviewRepository.FindAll(x => x.ProductId == request.ProductId).ToList();

            // Lấy danh sách UserId từ các review
            var userIds = productReviews.Select(x => x.UserId).Distinct().ToList();

            // Lấy thông tin User từ UserRepository
            var users = userRepository.FindAll(u => userIds.Contains(u.Id))
                                      .ToDictionary(u => u.Id, u => u); // Chuyển thành Dictionary để tra cứu nhanh

            // Chuyển đổi dữ liệu sang DTO
            var productReviewDTOs = productReviews.Select(review => new ProductReviewDTO
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
                    : null!
            }).ToList();

            return productReviewDTOs;
        }
    }
}
