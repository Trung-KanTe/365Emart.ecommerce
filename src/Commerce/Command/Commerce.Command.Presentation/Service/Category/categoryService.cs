using Asp.Versioning;
using Commerce.Command.Application.CategoryCases.Category;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.Category;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.Category
{
    /// <summary>
    /// Controller version 1 for category apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.CATEGORY_ROUTE)]
    [Authorize(Roles = "ADMIN,STAFF")]
    public class categoryService : ApiController
    {
        private readonly IMediator mediator;

        public categoryService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create category
        /// </summary>
        /// <param name="command">Request to create category</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update category
        /// </summary>
        /// <param name="id">Id of category need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(Guid? id, [FromBody] UpdateCategoryRequestDTO request)
        {
            var command = new UpdateCategoryCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete category
        /// </summary>
        /// <param name="id">id of category</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(Guid? id)
        {
            var command = new DeleteCategoryCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}