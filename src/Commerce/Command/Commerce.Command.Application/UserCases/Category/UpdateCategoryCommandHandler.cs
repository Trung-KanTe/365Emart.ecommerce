using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Entities = Commerce.Command.Domain.Entities.Category;
using Commerce.Command.Domain.Abstractions.Repositories.Category;
using MediatR;
using Commerce.Command.Domain.Entities.Category;

namespace Commerce.Command.Application.CategoryCases.Category
{
    /// <summary>
    /// Request to delete category, contain category id
    /// </summary>
    public record UpdateCategoryCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; } = null;
        public string? Style { get; set; } = null;
        public Guid? UserId { get; set; }
        public int? Views { get; set; } = 0;
        public bool? IsDeleted { get; set; } = false;
        public List<Guid>? ClassificationIds { get; set; }
    }

    /// <summary>
    /// Handler for delete category request
    /// </summary>
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
    {
        private readonly ICategoryRepository categoryRepository;

        /// <summary>
        /// Handler for delete category request
        /// </summary>
        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Handle delete category request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await categoryRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete category
                var category = await categoryRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (category == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Category)) })));
                }
                // Update category, keep original data if request is null
                request.MapTo(category, true);
                category.ValidateUpdate();

                category.ClassificationCategories = request.ClassificationIds?.Distinct().Select(classificationId => new ClassificationCategory
                {
                    CategoryId = category.Id,
                    ClassificationId = classificationId,
                }).ToList() ?? category.ClassificationCategories;
                // Mark category as Updated state
                categoryRepository.Update(category);
                // Save category to database
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