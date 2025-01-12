using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Application.UserCases.User
{
    /// <summary>
    /// Request to get user by id
    /// </summary>
    public record GetUserByIdQuery : IRequest<Result<Entities.User>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get user by id request
    /// </summary>
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<Entities.User>>
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Handler for get user by id request
        /// </summary>
        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with user data</returns>
        public async Task<Result<Entities.User>> Handle(GetUserByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            // Find user without allow null return. If user not found will throw NotFoundException
            var user = await userRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            return user!;
        }
    }
}
