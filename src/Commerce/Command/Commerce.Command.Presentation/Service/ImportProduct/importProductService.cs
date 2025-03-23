using Asp.Versioning;
using Commerce.Command.Application.UserCases.ImportProduct;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.ImportProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.ImportProduct
{
    /// <summary>
    /// Controller version 1 for importProduct apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.IMPORT_PRODUCT_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class importProductService : ApiController
    {
        private readonly IMediator mediator;

        public importProductService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create importProduct
        /// </summary>
        /// <param name="command">Request to create importProduct</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateImportProduct(CreateImportProductCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update importProduct
        /// </summary>
        /// <param name="id">Id of importProduct need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateImportProduct(Guid? id, [FromBody] UpdateImportProductRequestDTO request)
        {
            var command = new UpdateImportProductCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete importProduct
        /// </summary>
        /// <param name="id">id of importProduct</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteImportProduct(Guid? id)
        {
            var command = new DeleteImportProductCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}