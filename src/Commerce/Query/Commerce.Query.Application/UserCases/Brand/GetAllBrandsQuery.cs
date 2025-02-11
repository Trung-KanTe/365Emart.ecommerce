using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Brand;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Brand;

namespace Commerce.Query.Application.UserCases.Brand
{
    /// <summary>
    /// Request to get all brand
    /// </summary>
    public class GetAllBrandsQuery : IRequest<Result<List<Entities.Brand>>>
    {
    }

    /// <summary>
    /// Handler for get all brand request
    /// </summary>
    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, Result<List<Entities.Brand>>>
    {
        private readonly IBrandRepository brandRepository;

        /// <summary>
        /// Handler for get all brand request
        /// </summary>
        public GetAllBrandsQueryHandler(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list brand as data</returns>
        public async Task<Result<List<Entities.Brand>>> Handle(GetAllBrandsQuery request,
                                                       CancellationToken cancellationToken)
        {
            var brands = brandRepository.FindAll().ToList();
            return await Task.FromResult(brands);
        }
    }
}
