using Asp.Versioning;
using Commerce.Query.Application.CategoryCases.Category;
using Commerce.Query.Application.UserCases.Category;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Category
{
    /// <summary>
    /// Controller version 1 for category apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.CATEGORY_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class categoryService : ApiController
    {
        private readonly IMediator mediator;

        public categoryService(IMediator mediator)
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
        public async Task<IActionResult> GetCategoryById(Guid? id)
        {
            var query = new GetCategoryByIdQuery
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
        public async Task<IActionResult> GetAllCategorys([FromQuery] int pageNumber)
        {
            var query = new GetAllCategoryQuery(pageNumber);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("name")]
        public async Task<IActionResult> GetAllCategorys()
        {
            var query = new GetAllCategorysQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("all-product")]
        public async Task<IActionResult> GetAllProductByCategoryId([FromQuery] Guid? id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            var query = new GetAllProductByIdQuery(id, pageNumber, pageSize);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("search")]
        public async Task<IActionResult> SearchCategory([FromQuery] string name, [FromQuery] int pageNumber = 1)
        {
            var query = new SearchCategoryQuery(name, pageNumber);
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}