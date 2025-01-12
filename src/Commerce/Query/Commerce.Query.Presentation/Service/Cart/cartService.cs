using Asp.Versioning;
using Commerce.Query.Application.UserCases.Cart;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Cart
{
    /// <summary>
    /// Controller version 1 for cart apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.CART_ROUTE)]
    [Authorize(Roles = "ADMIN,STAFF")]
    public class cartService : ApiController
    {
        private readonly IMediator mediator;

        public cartService(IMediator mediator)
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
        public async Task<IActionResult> GetCartById(Guid? id)
        {
            var query = new GetCartByIdQuery
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
        public async Task<IActionResult> GetAllCarts()
        {
            var query = new GetAllCartQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}