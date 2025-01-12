using Asp.Versioning;
using Commerce.Command.Application.UserCases.Order;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.Order;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.Order
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
        /// Api version 1 for create orderCancel
        /// </summary>
        /// <param name="command">Request to create orderCancel</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrderCancel(CreateOrderCancelCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update orderCancel
        /// </summary>
        /// <param name="id">Id of orderCancel need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateOrderCancel(Guid? id, [FromBody] UpdateOrderCancelRequestDTO request)
        {
            var command = new UpdateOrderCancelCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete orderCancel
        /// </summary>
        /// <param name="id">id of orderCancel</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteOrderCancel(Guid? id)
        {
            var command = new DeleteOrderCancelCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}