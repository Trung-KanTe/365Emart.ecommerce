using Asp.Versioning;
using Commerce.Command.Application.UserCases.Wallets;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Command.Presentation.Service.ShopWallet
{
    /// <summary>
    /// Controller version 1 for wallet apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.WALLET_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class walletService : ApiController
    {
        private readonly IMediator mediator;

        public walletService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create wallet
        /// </summary>
        /// <param name="command">Request to create wallet</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateShopWallet(CreateShopWalletCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("withDraw")]
        public async Task<IActionResult> CreateWithdraw(CreateWithdrawCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("withDrawAdmin")]
        public async Task<IActionResult> CreateWithdrawAdmin(CreateWithdrawAdminCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}