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
using Commerce.Command.Contract.Abstractions;

namespace Commerce.Command.Application.CategoryCases.Category
{
    /// <summary>
    /// Request to delete category, contain category id
    /// </summary>
    public record UpdateCategoryCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; } 
        public string? Style { get; set; } 
        public Guid? UserId { get; set; }
        public int? Views { get; set; } 
        public bool? IsDeleted { get; set; }
        public List<Guid>? ClassificationIds { get; set; }
    }

    /// <summary>
    /// Handler for delete category request
    /// </summary>
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IFileService fileService;

        /// <summary>
        /// Handler for delete category request
        /// </summary>
        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IFileService fileService)
        {
            this.categoryRepository = categoryRepository;
            this.fileService = fileService;
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
                var category = await categoryRepository.FindByIdAsync(request.Id.Value, true, cancellationToken, includeProperties: x=> x.ClassificationCategories!);
                if (category == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Category)) })));
                }
                // Update category, keep original data if request is null
                request.MapTo(category, true);
                if (request.Image is not null)
                {
                    string relativePath = "categories";
                    // Upload ảnh và lấy đường dẫn lưu trữ
                    string uploadedFilePath = await fileService.UploadFile(request.Name!, request.Image, relativePath);
                    // Cập nhật đường dẫn Icon
                    category!.Image = uploadedFilePath;
                }

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