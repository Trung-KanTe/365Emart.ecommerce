using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.User;

namespace Commerce.Command.Domain.Entities.User
{
    public class Role : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the Role
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description of the Role
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Implement the Validate method from Entity<Guid>
        /// </summary>
        public override void Validate()
        {
            ValidateCreate();
            ValidateUpdate();
        }

        /// <summary>
        /// Validate when creating a new User
        /// </summary>
        public void ValidateCreate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(UserConst.ROLE_NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Description)!.MaxLength(UserConst.ROLE_DESCRIPTION_MAX_LENGTH);
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing User
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.Name).NotEmpty().MaxLength(UserConst.ROLE_NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Description)!.MaxLength(UserConst.ROLE_DESCRIPTION_MAX_LENGTH);
            validator.Validate();
        }
    }
}
