﻿using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Category;
using Entities = Commerce.Command.Domain.Entities.Category;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Domain.Entities.Category;
using Commerce.Command.Contract.Abstractions;

namespace Commerce.Command.Application.CategoryCases.Category
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateCategoryCommand : IRequest<Result<Entities.Category>>
    {
        public string? Name { get; set; }
        public string? Image { get; set; } 
        public string? Style { get; set; } 
        public Guid? UserId { get; set; }
        public int? Views { get; set; } = 0;
        public bool? IsDeleted { get; set; } = true;
        public List<Guid>? ClassificationIds { get; set; }
    }

    /// <summary>
    /// Handler for create category request
    /// </summary>
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<Entities.Category>>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IFileService fileService;

        /// <summary>
        /// Handler for create category request
        /// </summary>
        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IFileService fileService)
        {
            this.categoryRepository = categoryRepository;
            this.fileService = fileService;
        }

        /// <summary>
        /// Handle create category request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with category data</returns>
        public async Task<Result<Entities.Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            // Create new Category from request
            Entities.Category? category = request.MapTo<Entities.Category>();

            if (request.Image is not null)
            {
                string relativePath = "categories";
                // Upload ảnh và lấy đường dẫn lưu trữ
                string uploadedFilePath = await fileService.UploadFile(request.Name!, request.Image, relativePath);
                // Cập nhật đường dẫn Icon
                category!.Image = uploadedFilePath;
            }

            // Begin transaction
            using var transaction = await categoryRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                category.ClassificationCategories = request.ClassificationIds?.Distinct().Select(roleId => new ClassificationCategory
                {
                    CategoryId = category.Id,
                    ClassificationId = roleId
                }).ToList();
                // Add data
                categoryRepository.Create(category!);
                // Save data
                await categoryRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return category;
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