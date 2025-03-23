using Commerce.Command.Contract.Abstractions;
using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Services;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Settings;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Entities = Commerce.Command.Domain.Entities.User;

namespace Commerce.Command.Application.UserCases.User
{
    /// <summary>
    /// Request to delete user, contain user id
    /// </summary>
    public record UserForgotPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Tel {  get; set; }
    }

    /// <summary>
    /// Handler for delete user request
    /// </summary>
    public class UserForgotPasswordCommandHandler : IRequestHandler<UserForgotPasswordCommand, Result>
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailSender emailSender;
        private readonly IJwtService jwtService;
        private readonly IRoleRepository roleRepository;

        /// <summary>
        /// Handler for delete user request
        /// </summary>
        public UserForgotPasswordCommandHandler(IUserRepository userRepository, IEmailSender emailSender, IJwtService jwtService, IRoleRepository roleRepository)
        {
            this.userRepository = userRepository;
            this.emailSender = emailSender;
            this.jwtService = jwtService;
            this.roleRepository = roleRepository;
        }

        /// <summary>
        /// Handle delete user request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UserForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await userRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete user
                var user = await userRepository.FindSingleAsync(x => x.Email == request.Email && x.Tel == request.Tel, true, cancellationToken, includeProperties: x => x.UserRoles!);
                if (user == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.User)) })));
                }
                var roleNames = roleRepository.FindAll(x => user.UserRoles.Select(role => role.RoleId).Contains(x.Id)).Select(roles => roles.Name).ToList();
                var token = jwtService.GenerateToken(user, roleNames!);
                BackgroundJob.Enqueue(() => emailSender.SendUserForgotPassword(user!.Email!, user.Name, token));
                // Commit transaction
                transaction.Commit();
                return Result.Ok();
            }
            catch (Exception)
            {
                // Rollback transaction
                transaction.Rollback();
                throw;
            }
        }
    }
}