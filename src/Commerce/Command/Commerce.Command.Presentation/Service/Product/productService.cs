using Asp.Versioning;
using Commerce.Command.Application.UserCases.DTOs;
using Commerce.Command.Application.UserCases.Product;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Commerce.Command.Presentation.Service.Product
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
        /// Api version 1 for create product
        /// </summary>
        /// <param name="command">Request to create product</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update product
        /// </summary>
        /// <param name="id">Id of product need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Guid? id, [FromBody] UpdateProductRequestDTO request)
        {
            var command = new UpdateProductCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete product
        /// </summary>
        /// <param name="id">id of product</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(Guid? id)
        {
            var command = new DeleteProductCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for create product
        /// </summary>
        /// <param name="command">Request to create product</param>
        /// <returns>Action result</returns>
        [HttpPut("view")]
        public async Task<IActionResult> UpdateProductView(Guid? id)
        {
            var command = new UpdateProductViewCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for create product
        /// </summary>
        /// <param name="command">Request to create product</param>
        /// <returns>Action result</returns>
        [HttpPut("stockQuantity")]
        public async Task<IActionResult> UpdateQuantityproductDetail([FromBody] UpdateQuantityProductDetailCommand request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }
    }
}