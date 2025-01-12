using Asp.Versioning;
using Commerce.Command.Application.UserCases.Brand;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.Brand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.Brand
{
    /// <summary>
    /// Controller version 1 for brand apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.BRAND_ROUTE)]
    [Authorize(Roles = "ADMIN,STAFF")]
    public class brandService : ApiController
    {
        private readonly IMediator mediator;

        public brandService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create brand
        /// </summary>
        /// <param name="command">Request to create brand</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBrand(CreateBrandCommandHandler command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update brand
        /// </summary>
        /// <param name="id">Id of brand need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateBrand(Guid? id, [FromBody] UpdateBrandRequestDTO request)
        {
            var command = new UpdateBrandCommand
            {
                Id = id,
                
            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete brand
        /// </summary>
        /// <param name="id">id of brand</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteBrand(Guid? id)
        {
            var command = new DeleteBrandCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}