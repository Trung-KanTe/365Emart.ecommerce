using Asp.Versioning;
using Commerce.Query.Application.UserCases.Province;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Province
{
    /// <summary>
    /// Controller version 1 for province apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.PROVINCE_ROUTE)]
    public class provinceService : ApiController
    {
        private readonly IMediator mediator;

        public provinceService(IMediator mediator)
        {
            this.mediator = mediator;
        }


        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet]
        public async Task<IActionResult> GetAllProvinces()
        {
            // Tạo query phân trang
            var query = new GetAllProvinceQuery();
            var result = await mediator.Send(query);

            return Ok(result);
        }
    }
}
