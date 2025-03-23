using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Ward;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Ward;

namespace Commerce.Query.Application.UserCases.Province
{
    /// <summary>
    /// Request to get all province
    /// </summary>
    public class GetAllProvinceQuery : IRequest<Result<List<Entities.Province>>>
    {
    }

    /// <summary>
    /// Handler for get all province request
    /// </summary>
    public class GetAllProvinceQueryHandler : IRequestHandler<GetAllProvinceQuery, Result<List<Entities.Province>>>
    {
        private readonly IProvinceRepository provinceRepository;

        /// <summary>
        /// Handler for get all province request
        /// </summary>
        public GetAllProvinceQueryHandler(IProvinceRepository provinceRepository)
        {
            this.provinceRepository = provinceRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list province as data</returns>
        public async Task<Result<List<Entities.Province>>> Handle(GetAllProvinceQuery request,
                                                       CancellationToken cancellationToken)
        {
            var provinces = provinceRepository.FindAll().ToList();
            return await Task.FromResult(provinces);
        }
    }
}
