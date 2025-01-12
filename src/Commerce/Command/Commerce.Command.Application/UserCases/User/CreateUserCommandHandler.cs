using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using Entities = Commerce.Command.Domain.Entities.User;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Domain.Entities.User;

namespace Commerce.Command.Application.UserCases.User
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateUserCommand : IRequest<Result<Entities.User>>
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Tel { get; set; }
        public string? Address { get; set; }
        public Guid? WardId { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public List<Guid>? RoleIds { get; set; }
    }

    /// <summary>
    /// Handler for create user request
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Entities.User>>
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Handler for create user request
        /// </summary>
        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Handle create user request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with user data</returns>
        public async Task<Result<Entities.User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Create new User from request
            Entities.User? user = request.MapTo<Entities.User>();
            // Validate for user
            user!.Validate();
            // Begin transaction
            using var transaction = await userRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                user.UserRoles = request.RoleIds?.Distinct().Select(roleId => new UserRole
                {
                    UserId = user.Id,  
                    RoleId = roleId     
                }).ToList();
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