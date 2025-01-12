using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Entities = Commerce.Command.Domain.Entities.Product;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;

namespace Commerce.Command.Application.UserCases.Product
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateProductReviewCommand : IRequest<Result<Entities.ProductReview>>
    {
        public Guid? ProductId { get; set; }
        public Guid? UserId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public bool? IsDeleted { get; set; }
    }

    /// <summary>
    /// Handler for create productReview request
    /// </summary>
    public class CreateProductReviewCommandHandler : IRequestHandler<CreateProductReviewCommand, Result<Entities.ProductReview>>
    {
        private readonly IProductReviewRepository productReviewRepository;

        /// <summary>
        /// Handler for create productReview request
        /// </summary>
        public CreateProductReviewCommandHandler(IProductReviewRepository productReviewRepository)
        {
            this.productReviewRepository = productReviewRepository;
        }

        /// <summary>
        /// Handle create productReview request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with productReview data</returns>
        public async Task<Result<Entities.ProductReview>> Handle(CreateProductReviewCommand request, CancellationToken cancellationToken)
        {
            // Create new ProductReview from request
            Entities.ProductReview? productReview = request.MapTo<Entities.ProductReview>();
            // Validate for productReview
            productReview!.ValidateCreate();
            // Begin transaction
            using var transaction = await productReviewRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Add data
                productReviewRepository.Create(productReview!);
                // Save data
                await productReviewRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return productReview;
            }
            catch (Exception)
            {
                // Rollback transaction
                transaction.Rollback();
                throw;
            }
        }
    }
}