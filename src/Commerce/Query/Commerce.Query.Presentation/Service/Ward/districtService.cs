using Asp.Versioning;
using Commerce.Query.Application.DistrictCases.District;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.District
{
    /// <summary>
    /// Controller version 1 for district apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.DISTRICT_ROUTE)]
    public class districtService : ApiController
    {
        private readonly IMediator mediator;

        public districtService(IMediator mediator)
        {
            this.mediator = mediator;
        }


        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("{provinceId:int}")]
        public async Task<IActionResult> GetDistrictById(int? provinceId)
        {
            var query = new GetDistrictByIdQuery
            {
                ProvinceId = provinceId
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}
