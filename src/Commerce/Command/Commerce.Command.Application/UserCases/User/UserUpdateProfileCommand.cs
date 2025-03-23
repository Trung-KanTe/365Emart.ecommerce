using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using MediatR;

namespace Commerce.Command.Application.UserCases.User
{
    /// <summary>
    /// Request to delete user, contain user id
    /// </summary>
    public record UserUpdateProfileCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Tel { get; set; }
        public string? Address { get; set; }
        public int? WardId { get; set; }
    }

    /// <summary>
    /// Handler for delete user request
    /// </summary>
    public class UserUpdateProfileCommandHandler : IRequestHandler<UserUpdateProfileCommand, Result>
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Handler for delete user request
        /// </summary>
        public UserUpdateProfileCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Handle delete user request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UserUpdateProfileCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await userRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete user
                var user = await userRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                
                // Update user, keep original data if request is null
                request.MapTo(user, true);
                // user.ValidateUpdate();
                // Mark user as Updated state
                userRepository.Update(user);
                // Save user to database
                await userRepository.SaveChangesAsync(cancellationToken);
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