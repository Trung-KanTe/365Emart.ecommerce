using Asp.Versioning;
using Commerce.Query.Application.UserCases.Brand;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Brand
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
        /// Api version 1 for get sample by id
        /// </summary>
        /// <param name="id">ID of sample</param>
        /// <returns>Action result with sample as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBrandById(Guid? id)
        {
            var query = new GetBrandByIdQuery
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
        public async Task<IActionResult> GetAllBrands()
        {
            var query = new GetAllBrandQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}