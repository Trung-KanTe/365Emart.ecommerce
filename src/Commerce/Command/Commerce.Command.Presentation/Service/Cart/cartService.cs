using Asp.Versioning;
using Commerce.Command.Application.UserCases.Cart;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.Cart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.Cart
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
        /// Api version 1 for create cart
        /// </summary>
        /// <param name="command">Request to create cart</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCart(CreateCartCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update cart
        /// </summary>
        /// <param name="id">Id of cart need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCart(Guid? id, [FromBody] UpdateCartRequestDTO request)
        {
            var command = new UpdateCartCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete cart
        /// </summary>
        /// <param name="id">id of cart</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCart(Guid? id)
        {
            var command = new DeleteCartCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}