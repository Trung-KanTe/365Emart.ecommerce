using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.Category;
using Entities = Commerce.Command.Domain.Entities.Category;
using MediatR;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;

namespace Commerce.Command.Application.CategoryCases.Category
{
    /// <summary>
    /// Request to delete category, contain category id
    /// </summary>
    public record DeleteCategoryCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete category, contain category id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete category request
    /// </summary>
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
    {
        private readonly ICategoryRepository categoryRepository;

        /// <summary>
        /// Handler for delete category request
        /// </summary>
        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Handle delete category request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await categoryRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete category
                var category = await categoryRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (category == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Category)) })));
                }
                category.IsDeleted = !category.IsDeleted;
                categoryRepository.Update(category);
                await categoryRepository.SaveChangesAsync(cancellationToken);

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