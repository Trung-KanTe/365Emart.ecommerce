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
    /// Controller version 1 for order apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.ORDER_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class orderService : ApiController
    {
        private readonly IMediator mediator;

        public orderService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create order
        /// </summary>
        /// <param name="command">Request to create order</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update order
        /// </summary>
        /// <param name="id">Id of order need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateOrder(Guid? id, [FromBody] UpdateOrderRequestDTO request)
        {
            var command = new UpdateOrderCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete order
        /// </summary>
        /// <param name="id">id of order</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(Guid? id)
        {
            var command = new DeleteOrderCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update order
        /// </summary>
        /// <param name="id">Id of order need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut("method")]
        public async Task<IActionResult> UpdateStatusOrder(Guid? id)
        {
            var command = new UpdateStatusOrderCommand
            {
                Id = id,

            };
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("now")]
        public async Task<IActionResult> CreateOrderNow(CreateOrderNowCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}