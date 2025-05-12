using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Entities = Commerce.Command.Domain.Entities.Product;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using MediatR;
using Commerce.Command.Contract.Abstractions;

namespace Commerce.Command.Application.UserCases.Product
{
    /// <summary>
    /// Request to delete productReview, contain productReview id
    /// </summary>
    public record UpdateProductReviewReplyCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public string? Reply { get; set; }
    }

    /// <summary>
    /// Handler for delete productReview request
    /// </summary>
    public class UpdateProductReviewReplyCommandHandler : IRequestHandler<UpdateProductReviewReplyCommand, Result>
    {
        private readonly IProductReviewReplyRepository productReviewReplyRepository;
        private readonly IProductRepository productRepository;
        private readonly IFileService fileService;

        /// <summary>
        /// Handler for delete productReview request
        /// </summary>
        public UpdateProductReviewReplyCommandHandler(IProductReviewReplyRepository productReviewReplyRepository, IProductRepository productRepository, IFileService fileService)
        {
            this.productReviewReplyRepository = productReviewReplyRepository;
            this.productRepository = productRepository;
            this.fileService = fileService;
        }

        /// <summary>
        /// Handle delete productReview request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateProductReviewReplyCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await productReviewReplyRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete productReview
                var productReview = await productReviewReplyRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (productReview == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.ProductReview)) })));
                }

                // Update productReview, keep original data if request is null
                request.MapTo(productReview, true);

                // Mark productReview as Updated state
                productReviewReplyRepository.Update(productReview);
                // Save productReview to database
                await productReviewReplyRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return Result.Ok();
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