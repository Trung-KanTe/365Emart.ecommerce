using Asp.Versioning;
using Commerce.Command.Application.UserCases.Shop;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.Shop;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.Shop
{
    /// <summary>
    /// Controller version 1 for shop apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.SHOP_ROUTE)]
    [Authorize(Roles = "ADMIN,STAFF")]
    public class shopService : ApiController
    {
        private readonly IMediator mediator;

        public shopService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create shop
        /// </summary>
        /// <param name="command">Request to create shop</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateShop(CreateShopCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update shop
        /// </summary>
        /// <param name="id">Id of shop need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateShop(Guid? id, [FromBody] UpdateShopRequestDTO request)
        {
            var command = new UpdateShopCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete shop
        /// </summary>
        /// <param name="id">id of shop</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteShop(Guid? id)
        {
            var command = new DeleteShopCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}