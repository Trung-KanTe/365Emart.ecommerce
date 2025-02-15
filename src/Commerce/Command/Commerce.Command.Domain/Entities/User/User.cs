using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Command.Domain.Entities.User
{
    public class User : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the User
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Email of the User
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Hashed Password of the User
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Phone number of the User
        /// </summary>
        public string? Tel { get; set; }

        /// <summary>
        /// Address of the User
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Ward ID for the User's location
        /// </summary>
        public int? WardId { get; set; }

        /// <summary>
        /// Date when the User was created
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// User ID of the person who created this User
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Date when the User was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User ID of the person who last updated this User
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Status indicating if the User is deleted (soft delete)
        /// </summary>
        public bool? IsDeleted { get; set; } = true;

        /// <summary>
        /// List ClassificationCategory of Category
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public ICollection<UserRole>? UserRoles { get; set; }

        /// <summary>
        /// Implement the Validate method from Entity<Guid>
        /// </summary>
        public override void Validate()
        {
            ValidateCreate();
            ValidateUpdate();
            ValidateLogin();
        }

        /// <summary>
        /// Validate when creating a new User
        /// </summary>
        public void ValidateCreate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(UserConst.USER_NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Email).NotNull()!.NotEmpty().IsEmail();
            validator.RuleFor(x => x.Tel)!.NotEmpty().IsPhoneNumber().MaxLength(UserConst.USER_TEL_MAX_LENGTH);
            validator.RuleFor(x => x.PasswordHash).NotNull()!.NotEmpty().MaxLength(UserConst.USER_PASSWORD_HASH_MAX_LENGTH);
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing User
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.Name).NotEmpty().MaxLength(UserConst.USER_NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Email).NotEmpty().IsEmail();
            validator.RuleFor(x => x.Tel)!.NotEmpty().IsPhoneNumber().MaxLength(UserConst.USER_TEL_MAX_LENGTH);
            validator.RuleFor(x => x.PasswordHash).NotEmpty().MaxLength(UserConst.USER_PASSWORD_HASH_MAX_LENGTH);
            validator.Validate();
        }

        /// <summary>
        /// Validate when creating a new User
        /// </summary>
        public void ValidateLogin()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.Email).NotNull()!.NotEmpty().IsEmail();
            validator.RuleFor(x => x.PasswordHash).NotNull()!.NotEmpty().MaxLength(UserConst.USER_PASSWORD_HASH_MAX_LENGTH);
            validator.Validate();
        }
    }
}
