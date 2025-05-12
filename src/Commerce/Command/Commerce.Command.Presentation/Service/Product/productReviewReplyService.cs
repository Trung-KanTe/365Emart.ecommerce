using Asp.Versioning;
using Commerce.Command.Application.UserCases.Product;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.Product
{
    /// <summary>
    /// Controller version 1 for productReview apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.PRODUCT_REVIEW_REPLY_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class productReviewReplyService : ApiController
    {
        private readonly IMediator mediator;

        public productReviewReplyService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create productReview
        /// </summary>
        /// <param name="command">Request to create productReview</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateProductReviewReply(CreateProductReviewReplyCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductReviewReply(Guid? id, [FromBody] UpdateProductReviewReplyRequest request)
        {
            var command = new UpdateProductReviewReplyCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}