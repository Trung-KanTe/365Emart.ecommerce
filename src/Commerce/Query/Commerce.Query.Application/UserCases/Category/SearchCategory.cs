using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Category;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.Category;

namespace Commerce.Query.Application.CategoryCases.Category
{
    /// <summary>
    /// Request to get all category
    /// </summary>
    public class SearchCategoryQuery : IRequest<Result<PaginatedResult<CategoryDTO>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public string Name { get; set; }

        public SearchCategoryQuery(string Name, int pageNumber)
        {
            this.Name = Name;
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all category request
    /// </summary>
    public class SearchCategoryQueryHandler : IRequestHandler<SearchCategoryQuery, Result<PaginatedResult<CategoryDTO>>>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IClassificationRepository classificationRepository;
        private readonly IClassificationCategoryRepository classiCateRepository;

        public SearchCategoryQueryHandler(ICategoryRepository categoryRepository, IClassificationRepository classificationRepository, IClassificationCategoryRepository classiCateRepository)
        {
            this.categoryRepository = categoryRepository;
            this.classificationRepository = classificationRepository;
            this.classiCateRepository = classiCateRepository;
        }

        public async Task<Result<PaginatedResult<CategoryDTO>>> Handle(SearchCategoryQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 5;
            string searchTerm = request.Name?.Trim().ToLower()!;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.Category, bool>> predicate = p => string.IsNullOrEmpty(searchTerm) || p.Name.ToLower().Contains(searchTerm);

            // Gọi phương thức GetPaginatedResultAsync từ repository để lấy dữ liệu phân trang
            var paginatedCategorys = await categoryRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                predicate,
                isTracking: false,
                includeProperties: Array.Empty<Expression<Func<Entities.Category, object>>>()
            );

            var categoryIds = paginatedCategorys.Items.Select(c => c.Id).ToList();
            var classificationCategories = classiCateRepository
                .FindAll(cc => categoryIds.Contains(cc.CategoryId!.Value))
                .ToList();

            // Lấy các Classification từ bảng Classification tương ứng với ClassificationCategory
            var classificationIds = classificationCategories.Select(cc => cc.ClassificationId).Distinct().ToList();
            var classifications = classificationRepository
                .FindAll(c => classificationIds.Contains(c.Id))
                .ToList();

            // Chuyển đổi dữ liệu thành CategoryDTO và gắn các Classification vào từng Category
            var categoryDTOs = paginatedCategorys.Items.Select(category =>
            {
                var categoryDTO = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Image = category.Image,
                    Style = category.Style,
                    UserId = category.UserId,
                    Views = category.Views,
                    InsertedAt = category.InsertedAt,
                    IsDeleted = category.IsDeleted,
                    Classifications = classifications
                        .Where(c => classificationCategories.Any(cc => cc.CategoryId == category.Id && cc.ClassificationId == c.Id))
                        .ToList()
                };
                return categoryDTO;
            }).ToList();

            // Trả về kết quả dưới dạng Result<PaginatedResult<CategoryDTO>>
            var result = new PaginatedResult<CategoryDTO>(
                pageNumber,
                pageSize,
                paginatedCategorys.TotalCount,
                categoryDTOs
            );

            return await Task.FromResult(result);
        }
    }
}
