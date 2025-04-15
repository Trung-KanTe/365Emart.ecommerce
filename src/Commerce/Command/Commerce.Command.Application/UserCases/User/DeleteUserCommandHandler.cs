using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using Entities = Commerce.Command.Domain.Entities.User;
using MediatR;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;

namespace Commerce.Command.Application.UserCases.User
{
    /// <summary>
    /// Request to delete user, contain user id
    /// </summary>
    public record DeleteUserCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete user, contain user id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete user request
    /// </summary>
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Handler for delete user request
        /// </summary>
        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Handle delete user request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await userRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete user
                var user = await userRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (user == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.User)) })));
                }
                user.IsDeleted = !user.IsDeleted;
                userRepository.Update(user);
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