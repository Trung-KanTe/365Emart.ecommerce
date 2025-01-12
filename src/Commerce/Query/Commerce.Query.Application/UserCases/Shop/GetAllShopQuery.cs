using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Shop;

namespace Commerce.Query.Application.UserCases.Shop
{
    /// <summary>
    /// Request to get all shop
    /// </summary>
    public class GetAllShopQuery : IRequest<Result<List<Entities.Shop>>>
    {
    }

    /// <summary>
    /// Handler for get all shop request
    /// </summary>
    public class GetAllShopQueryHandler : IRequestHandler<GetAllShopQuery, Result<List<Entities.Shop>>>
    {
        private readonly IShopRepository shopRepository;

        /// <summary>
        /// Handler for get all shop request
        /// </summary>
        public GetAllShopQueryHandler(IShopRepository shopRepository)
        {
            this.shopRepository = shopRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list shop as data</returns>
        public async Task<Result<List<Entities.Shop>>> Handle(GetAllShopQuery request,
                                                       CancellationToken cancellationToken)
        {
            var shops = shopRepository.FindAll().ToList();
            return await Task.FromResult(shops);
        }
    }
}
