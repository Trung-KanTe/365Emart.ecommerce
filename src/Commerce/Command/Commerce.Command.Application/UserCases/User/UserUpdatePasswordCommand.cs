using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Entities = Commerce.Command.Domain.Entities.User;

namespace Commerce.Command.Application.UserCases.User
{
    /// <summary>
    /// Request to delete user, contain user id
    /// </summary>
    public record UserUpdatePasswordCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete user request
    /// </summary>
    public class UserUpdatePasswordCommandHandler : IRequestHandler<UserUpdatePasswordCommand, Result>
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Handler for delete user request
        /// </summary>
        public UserUpdatePasswordCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Handle delete user request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UserUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await userRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Lấy user từ DB
                var user = await userRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);                      

                return Result.Ok();
            }
            catch (Exception)
            {
                // Rollback nếu có lỗi
                transaction.Rollback();
                throw;
            }
        }
    }
}