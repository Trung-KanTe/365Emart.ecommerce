using Asp.Versioning;
using Commerce.Query.Application.UserCases.Promotion;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Promotion
{
    /// <summary>
    /// Controller version 1 for promotion apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.PROMOTION_ROUTE)]
    [Authorize(Roles = "ADMIN,STAFF")]
    public class promotionService : ApiController
    {
        private readonly IMediator mediator;

        public promotionService(IMediator mediator)
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
        public async Task<IActionResult> GetPromotionById(Guid? id)
        {
            var query = new GetPromotionByIdQuery
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
        public async Task<IActionResult> GetAllPromotions()
        {
            var query = new GetAllPromotionQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}