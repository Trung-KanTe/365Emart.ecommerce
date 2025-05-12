using Asp.Versioning;
using Commerce.Query.Application.UserCases.Wallets;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Shop
{
    /// <summary>
    /// Controller version 1 for wallet apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.WALLET_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class walletService : ApiController
    {
        private readonly IMediator mediator;

        public walletService(IMediator mediator)
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
        public async Task<IActionResult> GetWalletByShopId(Guid? id)
        {
            var query = new GetAllWalletByShopQuery
            {
                Id = id
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get sample by id
        /// </summary>
        /// <param name="id">ID of sample</param>
        /// <returns>Action result with sample as data</returns>
        [MapToApiVersion(1)]
        [HttpGet]
        public async Task<IActionResult> GetWalletByShop()
        {
            var query = new GetAllWalletByAdminQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}