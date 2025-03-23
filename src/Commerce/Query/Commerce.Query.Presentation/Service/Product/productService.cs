using Asp.Versioning;
using Commerce.Query.Application.UserCases.Product;
using Commerce.Query.Contract.Helpers;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Product
{
    /// <summary>
    /// Controller version 1 for product apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.PRODUCT_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class productService : ApiController
    {
        private readonly IMediator mediator;

        public productService(IMediator mediator)
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
        public async Task<IActionResult> GetProductById(Guid? id)
        {
            var query = new GetProductByIdQuery
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
        public async Task<IActionResult> GetAllProducts()
        {
            var query = new GetAllProductQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpPost("filter-products")]
        public async Task<IActionResult> GetFilterProducts([FromBody] SearchCommand request, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var searchCommand = new SearchCommand
            {
                FilterParams = request.FilterParams.Select(f => new FilterParam
                {
                    Name = f.Name,
                    Value = f.Value,
                    Condition = f.Condition
                }).ToList(),
                OrderParams = request.OrderParams.Select(o => new OrderParam
                {
                    Name = o.Name,
                    OrderDirection = o.OrderDirection
                }).ToList(),
            };

            var query = new GetFilterProductQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchCommand = searchCommand
            };

            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllProductsPaging([FromQuery] int pageNumber)
        {
            var query = new GetAllProductPagingQuery(pageNumber);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProduct([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            var query = new GetAllProductsQuery(pageNumber, pageSize);
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}