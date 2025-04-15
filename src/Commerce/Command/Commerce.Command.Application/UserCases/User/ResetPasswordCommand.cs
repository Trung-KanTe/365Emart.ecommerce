using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using MediatR;

namespace Commerce.Command.Application.UserCases.User
{
    /// <summary>
    /// Request to delete user, contain user id
    /// </summary>
    public record ResetPasswordCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public string? PasswordHash { get; set; }
    }

    /// <summary>
    /// Handler for delete user request
    /// </summary>
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Handler for delete user request
        /// </summary>
        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Handle delete user request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await userRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete user
                var user = await userRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
               
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