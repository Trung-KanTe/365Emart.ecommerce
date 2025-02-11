using Asp.Versioning;
using Commerce.Query.Application.UserCases.Classification;
using Commerce.Query.Application.UserCases.Classification;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Classification
{
    /// <summary>
    /// Controller version 1 for classification apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.CLASSIFICATION_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class classificationService : ApiController
    {
        private readonly IMediator mediator;

        public classificationService(IMediator mediator)
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
        public async Task<IActionResult> GetClassificationById(Guid? id)
        {
            var query = new GetClassificationByIdQuery
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
        public async Task<IActionResult> GetAllClassifications([FromQuery] int pageNumber)
        {
            // Tạo query phân trang
            var query = new GetAllClassificationQuery(pageNumber);
            var result = await mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("name")]
        public async Task<IActionResult> GetAllClassifications()
        {
            var query = new GetAllClassificationsQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}