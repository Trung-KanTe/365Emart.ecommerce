using Asp.Versioning;
using Commerce.Query.Application.ShopCases.Shop;
using Commerce.Query.Application.UserCases.Shop;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Query.Presentation.Service.Shop
{
    /// <summary>
    /// Controller version 1 for shop apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.SHOP_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class shopService : ApiController
    {
        private readonly IMediator mediator;

        public shopService(IMediator mediator)
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
        public async Task<IActionResult> GetShopById(Guid? id)
        {
            var query = new GetShopByIdQuery
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
        public async Task<IActionResult> GetAllShops()
        {
            var query = new GetAllShopQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllShopsPaging([FromQuery] int pageNumber)
        {
            var query = new GetAllShopPagingQuery(pageNumber);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("all-product")]
        public async Task<IActionResult> GetAllProductByShopId([FromQuery] Guid? id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
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
        [HttpGet("all-product-userId")]
        public async Task<IActionResult> GetAllProductByUserId([FromQuery] Guid? id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            var query = new GetAllProductByUserIdQuery(id, pageNumber, pageSize);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueByUserId([FromQuery] Guid? id)
        {
            var query = new GetRevenueByUserIdQuery(id);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("best-seller")]
        public async Task<IActionResult> GetBestSellerByUserId([FromQuery] Guid? id)
        {
            var query = new GetBestSellerByUserIdQuery(id);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("inventory")]
        public async Task<IActionResult> GetInventoryByUserId([FromQuery] Guid? id)
        {
            var query = new GetInventoryByUserIdQuery(id);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("all-revenue")]
        public async Task<IActionResult> GetAllRevenue()
        {
            var query = new GetAllRevenueQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("all-best-seller")]
        public async Task<IActionResult> GetAllBestSeller()
        {
            var query = new GetAllBestSellerQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("all-inventory")]
        public async Task<IActionResult> GetAllInventory()
        {
            var query = new GetAllInventoryQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("search")]
        public async Task<IActionResult> SearchShop([FromQuery] string name, [FromQuery] int pageNumber = 1)
        {
            var query = new SearchShopQuery(name, pageNumber);
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}