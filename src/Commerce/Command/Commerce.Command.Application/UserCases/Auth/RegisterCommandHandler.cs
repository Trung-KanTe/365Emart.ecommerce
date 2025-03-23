using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using Commerce.Command.Domain.Entities.User;
using MediatR;
using Entities = Commerce.Command.Domain.Entities.User;

namespace Commerce.commandApplication.UserCases.Auth
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record RegisterCommand : IRequest<Result<Entities.User>>
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Tel { get; set; }
        public string? Address { get; set; }
        public int? WardId { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }

    /// <summary>
    /// Handler for create user request
    /// </summary>
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<Entities.User>>
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Handler for create user request
        /// </summary>
        public RegisterCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Handle create user request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with user data</returns>
        public async Task<Result<Entities.User>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Create new User from request
            Entities.User? user = request.MapTo<Entities.User>();
            // Validate for user
            user!.ValidateCreate();
            // Begin transaction
            using var transaction = await userRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                user.UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        UserId = user.Id,
                        RoleId = new Guid("5E990A15-01D2-456F-298F-08DD1E3D9640")
                    }
                };
                // Add data
                userRepository.Create(user!);
                // Save data
                await userRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return user;
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