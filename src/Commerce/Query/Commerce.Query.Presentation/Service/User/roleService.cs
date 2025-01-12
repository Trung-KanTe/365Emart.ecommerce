using Asp.Versioning;
using Commerce.Query.Application.UserCases.Role;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Role
{
    /// <summary>
    /// Controller version 1 for role apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.ROLE_ROUTE)]
    [Authorize(Roles = "ADMIN,STAFF")]
    public class roleService : ApiController
    {
        private readonly IMediator mediator;

        public roleService(IMediator mediator)
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
        public async Task<IActionResult> GetRoleById(Guid? id)
        {
            var query = new GetRoleByIdQuery
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
        public async Task<IActionResult> GetAllRoles()
        {
            var query = new GetAllRoleQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}