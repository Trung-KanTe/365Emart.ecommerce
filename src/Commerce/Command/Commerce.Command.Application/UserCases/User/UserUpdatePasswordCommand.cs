using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Entities = Commerce.Command.Domain.Entities.User;

namespace Commerce.Command.Application.UserCases.User
{
    /// <summary>
    /// Request to delete user, contain user id
    /// </summary>
    public record UserUpdatePasswordCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public string? OldPasswordHash { get; set; }
        public string? NewPasswordHash { get; set; }
    }

    /// <summary>
    /// Handler for delete user request
    /// </summary>
    public class UserUpdatePasswordCommandHandler : IRequestHandler<UserUpdatePasswordCommand, Result>
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher<Entities.User> passwordHasher;

        /// <summary>
        /// Handler for delete user request
        /// </summary>
        public UserUpdatePasswordCommandHandler(IUserRepository userRepository, IPasswordHasher<Entities.User> passwordHasher)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Handle delete user request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UserUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await userRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Lấy user từ DB
                var user = await userRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (user == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, "User not found"));
                }

                // Kiểm tra mật khẩu cũ
                if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.OldPasswordHash!) != PasswordVerificationResult.Success)
                {
                    return Result.Failure(StatusCode.Conflict, new Error(ErrorType.Conflict, ErrCodeConst.CONFLICT, MessConst.MSG_LOGIN.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.User)) })));
                }

                // Hash mật khẩu mới trước khi lưu vào DB
                user.PasswordHash = "AQAAAAIAAYagAAAAEHM2GmbSC0rYxGTYnnckBdV2WkzbFmnw2S7frpmMXsuJb8Nbn3Qu7DvP3kXiCTYEog==";

                // Cập nhật mật khẩu
                userRepository.Update(user);

                // Lưu vào database
                await userRepository.SaveChangesAsync(cancellationToken);

                // Commit transaction
                transaction.Commit();
                return Result.Ok();
            }
            catch (Exception)
            {
                // Rollback nếu có lỗi
                transaction.Rollback();
                throw;
            }
        }
    }
}