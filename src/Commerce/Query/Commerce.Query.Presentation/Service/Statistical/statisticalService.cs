using Asp.Versioning;
using Commerce.Query.Application.UserCases.Shop;
using Commerce.Query.Application.UserCases.Wallets;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Statistical
{
    /// <summary>
    /// Controller version 1 for wallet apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.STATISTICAL_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class statisticalService : ApiController
    {
        private readonly IMediator mediator;

        public statisticalService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for get sample by id
        /// </summary>
        /// <param name="id">ID of sample</param>
        /// <returns>Action result with sample as data</returns>
        [MapToApiVersion(1)]
        [HttpGet]
        public async Task<IActionResult> GetAllStatistical([FromQuery] int? month, [FromQuery] int? year, [FromQuery] Guid? userId)
        {
            var query = new GetAllStatisticalShopQuery(month, year, userId);
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}