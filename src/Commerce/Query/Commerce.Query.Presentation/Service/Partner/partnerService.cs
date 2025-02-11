using Asp.Versioning;
using Commerce.Query.Application.UserCases.Partner;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Partner
{
    /// <summary>
    /// Controller version 1 for partner apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.PARTNER_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class partnerService : ApiController
    {
        private readonly IMediator mediator;

        public partnerService(IMediator mediator)
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
        public async Task<IActionResult> GetPartnerById(Guid? id)
        {
            var query = new GetPartnerByIdQuery
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
        public async Task<IActionResult> GetAllPartners()
        {
            var query = new GetAllPartnerQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPartnersPaging([FromQuery] int pageNumber)
        {
            var query = new GetAllPartnerPagingQuery(pageNumber);
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}