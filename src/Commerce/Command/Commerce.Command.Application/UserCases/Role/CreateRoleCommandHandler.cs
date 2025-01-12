using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using Entities = Commerce.Command.Domain.Entities.User;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;

namespace Commerce.Command.Application.UserCases.User
{
    /// <summary>
    /// Request to create 
    /// </summary>
    public record CreateRoleCommand : IRequest<Result<Entities.Role>>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    /// <summary>
    /// Handler for create role request
    /// </summary>
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result<Entities.Role>>
    {
        private readonly IRoleRepository roleRepository;

        /// <summary>
        /// Handler for create role request
        /// </summary>
        public CreateRoleCommandHandler(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        /// <summary>
        /// Handle create role request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with role data</returns>
        public async Task<Result<Entities.Role>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            // Create new Role from request
            Entities.Role? role = request.MapTo<Entities.Role>();
            // Validate for role
            role!.ValidateCreate();

            // Begin transaction
            using var transaction = await roleRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Add data
                roleRepository.Create(role!);
                // Save data
                await roleRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return role;
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