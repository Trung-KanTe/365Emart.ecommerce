using Asp.Versioning;
using Commerce.Command.Application.UserCases.WareHouse;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.WareHouse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.WareHouse
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
        /// Api version 1 for create wareHouse
        /// </summary>
        /// <param name="command">Request to create wareHouse</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateWareHouse(CreateWareHouseCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update wareHouse
        /// </summary>
        /// <param name="id">Id of wareHouse need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateWareHouse(Guid? id, [FromBody] UpdateWareHouseRequestDTO request)
        {
            var command = new UpdateWareHouseCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete wareHouse
        /// </summary>
        /// <param name="id">id of wareHouse</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteWareHouse(Guid? id)
        {
            var command = new DeleteWareHouseCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}