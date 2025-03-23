using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Entities = Commerce.Command.Domain.Entities.Product;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Abstractions;

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
        public string? Image { get; set; }
        public string? Comment { get; set; }
        public bool? IsDeleted { get; set; } = true;
    }

    /// <summary>
    /// Handler for create productReview request
    /// </summary>
    public class CreateProductReviewCommandHandler : IRequestHandler<CreateProductReviewCommand, Result<Entities.ProductReview>>
    {
        private readonly IProductReviewRepository productReviewRepository;
        private readonly IProductRepository productRepository;
        private readonly IFileService fileService;

        /// <summary>
        /// Handler for create productReview request
        /// </summary>
        public CreateProductReviewCommandHandler(IProductReviewRepository productReviewRepository, IProductRepository productRepository, IFileService fileService)
        {
            this.productReviewRepository = productReviewRepository;
            this.productRepository = productRepository;
            this.fileService = fileService; 
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

            var product = await productRepository.FindByIdAsync(request.ProductId!.Value, true, cancellationToken);

            if (request.Image is not null)
            {
                string normalizedProductName = product!.Name!.Replace(" ", "_");
                string relativePath = "productReviews";
                string uploadedFilePath = await fileService.UploadFile(normalizedProductName, request.Image, relativePath);
                productReview!.Image = uploadedFilePath;
            }

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