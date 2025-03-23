using Asp.Versioning;
using Commerce.Command.Application.UserCases.User;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.User
{
    /// <summary>
    /// Controller version 1 for role apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.ROLE_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class roleService : ApiController
    {
        private readonly IMediator mediator;

        public roleService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create role
        /// </summary>
        /// <param name="command">Request to create role</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update role
        /// </summary>
        /// <param name="id">Id of role need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateRole(Guid? id, [FromBody] UpdateRoleRequestDTO request)
        {
            var command = new UpdateRoleCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete role
        /// </summary>
        /// <param name="id">id of role</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteRole(Guid? id)
        {
            var command = new DeleteRoleCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}