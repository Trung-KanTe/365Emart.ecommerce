using Asp.Versioning;
using Commerce.Command.Application.UserCases.Classification;
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
    /// Controller version 1 for classification apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.CLASSIFICATION_ROUTE)]
    [Authorize(Roles = "ADMIN,STAFF")]
    public class classificationService : ApiController
    {
        private readonly IMediator mediator;

        public classificationService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create classification
        /// </summary>
        /// <param name="command">Request to create classification</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateClassification(CreateClassificationCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update classification
        /// </summary>
        /// <param name="id">Id of classification need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateClassification(Guid? id, [FromBody] UpdateClassificationRequestDTO request)
        {
            var command = new UpdateClassificationCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete classification
        /// </summary>
        /// <param name="id">id of classification</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteClassification(Guid? id)
        {
            var command = new DeleteClassificationCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}