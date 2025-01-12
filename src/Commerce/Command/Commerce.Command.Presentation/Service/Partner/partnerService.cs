using Asp.Versioning;
using Commerce.Command.Application.UserCases.Partner;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.Partner;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.Partner
{
    /// <summary>
    /// Controller version 1 for partner apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.PARTNER_ROUTE)]
    [Authorize(Roles = "ADMIN,STAFF")]
    public class partnerService : ApiController
    {
        private readonly IMediator mediator;

        public partnerService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create partner
        /// </summary>
        /// <param name="command">Request to create partner</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePartner(CreatePartnerCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update partner
        /// </summary>
        /// <param name="id">Id of partner need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdatePartner(Guid? id, [FromBody] UpdatePartnerRequestDTO request)
        {
            var command = new UpdatePartnerCommand
            {
                Id = id,

            };
            request.MapTo(command, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete partner
        /// </summary>
        /// <param name="id">id of partner</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeletePartner(Guid? id)
        {
            var command = new DeletePartnerCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}