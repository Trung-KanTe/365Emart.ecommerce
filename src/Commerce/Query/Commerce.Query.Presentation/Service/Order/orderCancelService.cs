using Asp.Versioning;
using Commerce.Query.Application.UserCases.Order;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.OrderCancel
{
    /// <summary>
    /// Controller version 1 for orderCancel apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.ORDER_CANCEL_ROUTE)]
    [Authorize(Roles = "ADMIN,STAFF")]
    public class orderCancelService : ApiController
    {
        private readonly IMediator mediator;

        public orderCancelService(IMediator mediator)
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
        public async Task<IActionResult> GetOrderCancelById(Guid? id)
        {
            var query = new GetOrderCancelByIdQuery
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
        public async Task<IActionResult> GetAllOrderCancels()
        {
            var query = new GetAllOrderCancelQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}