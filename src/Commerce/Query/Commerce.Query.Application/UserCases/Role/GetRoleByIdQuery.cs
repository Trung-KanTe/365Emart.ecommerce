using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Application.UserCases.Role
{
    /// <summary>
    /// Request to get role by id
    /// </summary>
    public record GetRoleByIdQuery : IRequest<Result<Entities.Role>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get role by id request
    /// </summary>
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Result<Entities.Role>>
    {
        private readonly IRoleRepository roleRepository;

        /// <summary>
        /// Handler for get role by id request
        /// </summary>
        public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with role data</returns>
        public async Task<Result<Entities.Role>> Handle(GetRoleByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            // Find role without allow null return. If role not found will throw NotFoundException
            var role = await roleRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            return role!;
        }
    }
}
