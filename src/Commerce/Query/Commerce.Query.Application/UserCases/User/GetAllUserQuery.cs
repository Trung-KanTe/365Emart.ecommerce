using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Application.UserCases.User
{
    /// <summary>
    /// Request to get all user
    /// </summary>
    public class GetAllUserQuery : IRequest<Result<List<Entities.User>>>
    {
    }

    /// <summary>
    /// Handler for get all user request
    /// </summary>
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, Result<List<Entities.User>>>
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Handler for get all user request
        /// </summary>
        public GetAllUserQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list user as data</returns>
        public async Task<Result<List<Entities.User>>> Handle(GetAllUserQuery request,
                                                       CancellationToken cancellationToken)
        {
            var users = userRepository.FindAll().ToList();
            return await Task.FromResult(users);
        }
    }
}
