using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Entities = Commerce.Command.Domain.Entities.User;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using MediatR;
using Commerce.Command.Domain.Entities.User;

namespace Commerce.Command.Application.UserCases.User
{
    /// <summary>
    /// Request to delete user, contain user id
    /// </summary>
    public record UpdateUserCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Tel { get; set; }
        public string? Address { get; set; }
        public int? WardId { get; set; }
        public bool? IsDeleted { get; set; } 
        public ICollection<UserRole>? UserRoles { get; set; }
    }

    /// <summary>
    /// Handler for delete user request
    /// </summary>
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Handler for delete user request
        /// </summary>
        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Handle delete user request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await userRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete user
                var user = await userRepository.FindByIdAsync(request.Id.Value, true, cancellationToken, x => x.UserRoles!);
                if (user == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.User)) })));
                }
                // Update user, keep original data if request is null
                request.MapTo(user, true);
                user.ValidateUpdate();

                user.UserRoles = request.UserRoles?.Distinct().Select(roleId => new UserRole
                {
                    UserId = user.Id,
                    RoleId = roleId.RoleId,
                }).ToList() ?? user.UserRoles;
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