using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.WareHouse;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.WareHouse;

namespace Commerce.Query.Application.WareHouseCases.WareHouse
{
    /// <summary>
    /// Request to get all wareHouse
    /// </summary>
    public class GetAllWareHouseQuery : IRequest<Result<List<Entities.WareHouse>>>
    {
    }

    /// <summary>
    /// Handler for get all wareHouse request
    /// </summary>
    public class GetAllWareHouseQueryHandler : IRequestHandler<GetAllWareHouseQuery, Result<List<Entities.WareHouse>>>
    {
        private readonly IWareHouseRepository wareHouseRepository;

        /// <summary>
        /// Handler for get all wareHouse request
        /// </summary>
        public GetAllWareHouseQueryHandler(IWareHouseRepository wareHouseRepository)
        {
            this.wareHouseRepository = wareHouseRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list wareHouse as data</returns>
        public async Task<Result<List<Entities.WareHouse>>> Handle(GetAllWareHouseQuery request,
                                                       CancellationToken cancellationToken)
        {
            var wareHouses = wareHouseRepository.FindAll().Where(p => p.IsDeleted == true).ToList();
            return await Task.FromResult(wareHouses);
        }
    }
}
