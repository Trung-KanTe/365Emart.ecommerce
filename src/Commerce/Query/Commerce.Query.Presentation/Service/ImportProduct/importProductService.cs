using Asp.Versioning;
using Commerce.Query.Application.UserCases.ImportProduct;
using Commerce.Query.Application.UserCases.Product;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.ImportProduct
{
    /// <summary>
    /// Controller version 1 for importProduct apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.IMPORT_PRODUCT_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class importProductService : ApiController
    {
        private readonly IMediator mediator;

        public importProductService(IMediator mediator)
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
        public async Task<IActionResult> GetImportProductById(Guid? id)
        {
            var query = new GetImportProductByIdQuery
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
        public async Task<IActionResult> GetAllImportProducts()
        {
            var query = new GetAllImportProductQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllImportProductsPaging([FromQuery] int pageNumber)
        {
            var query = new GetAllImportProductPagingQuery(pageNumber);
            var result = await mediator.Send(query);
            return Ok(result);
        }


        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("userId")]
        public async Task<IActionResult> GetAllImportProductShopPaging([FromQuery] Guid? id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            var query = new GetAllImportProductShopPagingQuery(id, pageNumber, pageSize);
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}