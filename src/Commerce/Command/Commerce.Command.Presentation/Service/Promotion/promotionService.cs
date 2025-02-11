using Asp.Versioning;
using Commerce.Command.Application.UserCases.Promotion;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.Promotion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.Promotion
{
    /// <summary>
    /// Controller version 1 for promotion apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.PROMOTION_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class promotionService : ApiController
    {
        private readonly IMediator mediator;

        public promotionService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create promotion
        /// </summary>
        /// <param name="command">Request to create promotion</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePromotion(CreatePromotionCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update promotion
        /// </summary>
        /// <param name="id">Id of promotion need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdatePromotion(Guid? id, [FromBody] UpdatePromotionRequestDTO request)
        {
            var command = new UpdatePromotionCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete promotion
        /// </summary>
        /// <param name="id">id of promotion</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeletePromotion(Guid? id)
        {
            var command = new DeletePromotionCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}