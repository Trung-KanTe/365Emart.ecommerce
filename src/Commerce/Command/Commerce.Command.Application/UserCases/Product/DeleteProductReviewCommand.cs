using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Entities = Commerce.Command.Domain.Entities.Product;
using MediatR;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;

namespace Commerce.Command.Application.UserCases.Product
{
    /// <summary>
    /// Request to delete productReview, contain productReview id
    /// </summary>
    public record DeleteProductReviewCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete productReview, contain productReview id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete productReview request
    /// </summary>
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteProductReviewCommand, Result>
    {
        private readonly IProductReviewRepository productReviewRepository;

        /// <summary>
        /// Handler for delete productReview request
        /// </summary>
        public DeleteCategoryCommandHandler(IProductReviewRepository productReviewRepository)
        {
            this.productReviewRepository = productReviewRepository;
        }

        /// <summary>
        /// Handle delete productReview request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(DeleteProductReviewCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await productReviewRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete productReview
                var productReview = await productReviewRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (productReview == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.ProductReview)) })));
                }
                productReview.IsDeleted = false;
                productReviewRepository.Update(productReview);
                await productReviewRepository.SaveChangesAsync(cancellationToken);

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