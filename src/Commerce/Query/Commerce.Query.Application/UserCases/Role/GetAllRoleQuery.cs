using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Application.UserCases.Role
{
    /// <summary>
    /// Request to get all role
    /// </summary>
    public class GetAllRoleQuery : IRequest<Result<List<Entities.Role>>>
    {
    }

    /// <summary>
    /// Handler for get all role request
    /// </summary>
    public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, Result<List<Entities.Role>>>
    {
        private readonly IRoleRepository roleRepository;

        /// <summary>
        /// Handler for get all role request
        /// </summary>
        public GetAllRoleQueryHandler(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list role as data</returns>
        public async Task<Result<List<Entities.Role>>> Handle(GetAllRoleQuery request,
                                                       CancellationToken cancellationToken)
        {
            var roles = roleRepository.FindAll().ToList();
            return await Task.FromResult(roles);
        }
    }
}
