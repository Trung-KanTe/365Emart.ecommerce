using Asp.Versioning;
using Commerce.Command.Application.UserCases.Product;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.Product
{
    /// <summary>
    /// Controller version 1 for productReview apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.PRODUCT_REVIEW_ROUTE)]
    [Authorize(Roles = "ADMIN,STAFF")]
    public class productReviewService : ApiController
    {
        private readonly IMediator mediator;

        public productReviewService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create productReview
        /// </summary>
        /// <param name="command">Request to create productReview</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateProductReview(CreateProductReviewCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update productReview
        /// </summary>
        /// <param name="id">Id of productReview need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateProductReview(Guid? id, [FromBody] UpdateProductReviewRequestDTO request)
        {
            var command = new UpdateProductReviewCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete productReview
        /// </summary>
        /// <param name="id">id of productReview</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteProductReview(Guid? id)
        {
            var command = new DeleteProductReviewCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}