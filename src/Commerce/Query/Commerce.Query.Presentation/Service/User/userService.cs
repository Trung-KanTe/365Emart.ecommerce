using Asp.Versioning;
using Commerce.Query.Application.UserCases.User;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.User
{
    /// <summary>
    /// Controller version 1 for user apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.USER_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class userService : ApiController
    {
        private readonly IMediator mediator;

        public userService(IMediator mediator)
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
        public async Task<IActionResult> GetUserById(Guid? id)
        {
            var query = new GetUserByIdQuery
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
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber)
        {
            // Tạo query phân trang
            var query = new GetAllUserQuery(pageNumber);
            var result = await mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("localization")]
        public async Task<IActionResult> GetAllUsersLocal ([FromQuery] int pageNumber)
        {
            // Tạo query phân trang
            var query = new GetAllUsersQuery(pageNumber);
            var result = await mediator.Send(query);

            return Ok(result);
        }
    }
}