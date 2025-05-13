using Asp.Versioning;
using Commerce.Query.Application.UserCases.GHTK;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Promotion
{
    /// <summary>
    /// Controller version 1 for promotion apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.SHIPPING_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class shippingService : ApiController
    {
        private readonly IMediator mediator;

        public shippingService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet]
        public async Task<IActionResult> GetShippingFee([FromQuery] Guid userId, [FromQuery] Guid orderId)
        {
            var query = new GetShippingFee
            {
                UserId = userId,
                OrderId = orderId
            };

            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}