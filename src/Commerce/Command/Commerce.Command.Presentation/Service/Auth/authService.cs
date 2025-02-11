using Asp.Versioning;
using Commerce.Command.Application.UserCases.User;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.commandApplication.UserCases.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Entities = Commerce.Command.Domain.Entities.User;

namespace Commerce.Command.Presentation.Service.Auth
{
    /// <summary>
    /// Controller version 1 for auth apis
    /// </summary>
    [ApiVersion(1)]
    public class authService : ApiController
    {
        private readonly IMediator mediator;
        private readonly IPasswordHasher<Entities.User> passwordHasher;

        public authService(IMediator mediator, IPasswordHasher<Entities.User> passwordHasher)
        {
            this.mediator = mediator;
            this.passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Api version 1 for create auth
        /// </summary>
        /// <param name="command">Request to create auth</param>
        /// <returns>Action result</returns>
        [Route(RouteConstant.API_PREFIX + RouteConstant.LOGIN_ROUTE)]
        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            Console.WriteLine($"Login - Email: {command.Email}");
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update auth
        /// </summary>
        /// <param name="id">Id of auth need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [Route(RouteConstant.API_PREFIX + RouteConstant.REGISTER_ROUTE)]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            Entities.User user = new Entities.User();
            command.PasswordHash = passwordHasher.HashPassword(user, command.PasswordHash!);
            command.MapTo(user, true);
            var result = await mediator.Send(command);
            return Ok(result);
        }      
    }
}
