using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Category;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Category;

namespace Commerce.Query.Application.UserCases.Category
{
    /// <summary>
    /// Request to get all category
    /// </summary>
    public class GetAllCategorysQuery : IRequest<Result<List<Entities.Category>>>
    {
    }

    /// <summary>
    /// Handler for get all category request
    /// </summary>
    public class GetAllCategorysQueryHandler : IRequestHandler<GetAllCategorysQuery, Result<List<Entities.Category>>>
    {
        private readonly ICategoryRepository categoryRepository;

        /// <summary>
        /// Handler for get all category request
        /// </summary>
        public GetAllCategorysQueryHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list category as data</returns>
        public async Task<Result<List<Entities.Category>>> Handle(GetAllCategorysQuery request,
                                                       CancellationToken cancellationToken)
        {
            var categorys = categoryRepository.FindAll().ToList();
            return await Task.FromResult(categorys);
        }
    }
}
