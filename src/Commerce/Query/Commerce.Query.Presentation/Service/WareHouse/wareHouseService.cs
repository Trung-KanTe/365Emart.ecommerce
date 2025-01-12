using Asp.Versioning;
using Commerce.Query.Application.WareHouseCases.WareHouse;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.WareHouse
{
    /// <summary>
    /// Controller version 1 for wareHouse apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.WARE_HOUSE_ROUTE)]
    [Authorize(Roles = "ADMIN,STAFF")]
    public class wareHouseService : ApiController
    {
        private readonly IMediator mediator;

        public wareHouseService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for get sample by id
        /// </summary>
        /// <param name="id">ID of sample</param>
        /// <returns>Action result with sample as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetWareHouseById(Guid? id)
        {
            var query = new GetWareHouseByIdQuery
            {
                Id = id
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet]
        public async Task<IActionResult> GetAllWareHouses()
        {
            var query = new GetAllWareHouseQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}