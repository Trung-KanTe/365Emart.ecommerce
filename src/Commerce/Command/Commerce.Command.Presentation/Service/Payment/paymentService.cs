using Asp.Versioning;
using Commerce.Command.Application.UserCases.Payment;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.Payment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.Payment
{
    /// <summary>
    /// Controller version 1 for payment apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.PAYMENT_ROUTE)]
    [Authorize(Roles = "ADMIN,STAFF")]
    public class paymentService : ApiController
    {
        private readonly IMediator mediator;

        public paymentService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create payment
        /// </summary>
        /// <param name="command">Request to create payment</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePayment(CreatePaymentCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update payment
        /// </summary>
        /// <param name="id">Id of payment need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdatePayment(Guid? id, [FromBody] UpdatePaymentRequestDTO request)
        {
            var command = new UpdatePaymentCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete payment
        /// </summary>
        /// <param name="id">id of payment</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeletePayment(Guid? id)
        {
            var command = new DeletePaymentCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}