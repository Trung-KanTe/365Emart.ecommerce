using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using Entities = Commerce.Command.Domain.Entities.User;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Domain.Abstractions.Repositories.Settings;
using Microsoft.AspNetCore.Identity;
using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;

namespace Commerce.Command.Application.UserCases.User
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record LoginCommand : IRequest<Result<AuthResult>>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }

    /// <summary>
    /// Handler for create user request
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResult>>
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IPasswordHasher<Entities.User> passwordHasher;
        private readonly IJwtService jwtService;

        /// <summary>
        /// Handler for create user request
        /// </summary>
        public LoginCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IPasswordHasher<Entities.User> passwordHasher, IJwtService jwtService)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.passwordHasher = passwordHasher;
            this.jwtService = jwtService;
        }

        /// <summary>
        /// Handle create user request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with user data</returns>
        public async Task<Result<AuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Create new User from request
            Entities.User? user = request.MapTo<Entities.User>();
            // Begin transaction
            user = await userRepository.FindSingleAsync(x => x.Email == request.UserName || x.Tel == request.UserName, true, cancellationToken, includeProperties: x => x.UserRoles!);
            if (user == null || passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password!) != PasswordVerificationResult.Success)
            {
                return Result.Failure(StatusCode.Conflict, new Error(ErrorType.Conflict, ErrCodeConst.CONFLICT, MessConst.MSG_LOGIN.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.User)) })));
            }

            var roleNames = roleRepository.FindAll(x => user.UserRoles.Select(role => role.RoleId).Contains(x.Id)).Select(roles => roles.Name).ToList();
            var token = jwtService.GenerateToken(user, roleNames!);

            return new AuthResult { Success = true, Token = token };
        }
    }
}