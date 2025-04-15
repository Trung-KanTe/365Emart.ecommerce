using Asp.Versioning;
using Commerce.Command.Application.UserCases.User;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Commerce.Command.Presentation.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Entities = Commerce.Command.Domain.Entities.User;

namespace Commerce.Command.Presentation.Service.User
{
    /// <summary>
    /// Controller version 1 for user apis
    /// </summary>
    /// [ApiController]
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.USER_ROUTE)]
    //[Authorize(Roles = "ADMIN,STAFF")]
    public class userService : ApiController
    {
        private readonly IMediator mediator;
        private readonly IPasswordHasher<Entities.User> passwordHasher;

        public userService(IMediator mediator, IPasswordHasher<Entities.User> passwordHasher)
        {
            this.mediator = mediator;
            this.passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Api version 1 for create user
        /// </summary>
        /// <param name="command">Request to create user</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand command)
        {
            Entities.User user = new Entities.User();
            if (string.IsNullOrWhiteSpace(command.PasswordHash))
            {
                command.PasswordHash = "12345";
            }
            command.PasswordHash = passwordHasher.HashPassword(user, command.PasswordHash!);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update user
        /// </summary>
        /// <param name="id">Id of user need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateUser(Guid? id, [FromBody] UpdateUserRequestDTO request)
        {
            var command = new UpdateUserCommand
            {
                Id = id,

            };            
            request.MapTo(command, true);
            if (request.PasswordHash is not null)
            {
                Entities.User user = new Entities.User();
                command.PasswordHash = passwordHasher.HashPassword(user, command.PasswordHash!);
            }
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete user
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid? id)
        {
            var command = new DeleteUserCommand
            {
                Id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update user
        /// </summary>
        /// <param name="id">Id of user need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut("profile")]
        public async Task<IActionResult> UserUpdateProfile(Guid? id, [FromBody] UpdateUserRequestDTO request)
        {
            var command = new UserUpdateProfileCommand
            {
                Id = id,

            };
            request.MapTo(command, true);          
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("password")]
        public async Task<IActionResult> UpdateUserPassword(Guid? id)
        {
            var command = new UserUpdatePasswordCommand
            {
                Id = id,            
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for create user
        /// </summary>
        /// <param name="command">Request to create user</param>
        /// <returns>Action result</returns>
        [HttpPost("forgot/password")]
        public async Task<IActionResult> UserForgotPassword(UserForgotPasswordCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update user
        /// </summary>
        /// <param name="id">Id of user need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword(Guid? id)
        {
            var command = new ResetPasswordCommand
            {
                Id = id,

            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}