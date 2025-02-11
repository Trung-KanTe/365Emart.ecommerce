using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.Category;
using Commerce.Command.Domain.Constants.User;

namespace Commerce.Command.Domain.Entities.Category
{
    /// <summary>
    /// Domain entity with int key type
    /// </summary>
    public class Classification : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of classification
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Icon of classification
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// Style of classification
        /// </summary>
        public string? Style { get; set; }

        /// <summary>
        /// Views of classification
        /// </summary>
        public int? Views { get; set; } = 0;

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
            validator.RuleFor(x => x.Name).NotNull()!.MaxLength(CategoryConst.NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Icon)!.NotVietnamese().MaxLength(CategoryConst.ICON_MAX_LENGTH);
            validator.RuleFor(x => x.Style)!.NotVietnamese().MaxLength(CategoryConst.STYLE_MAX_LENGTH);
            validator.RuleFor(x => x.Views).GreaterThanOrEqual(0);
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing User
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.Name)!.MaxLength(CategoryConst.NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Icon)!.NotVietnamese().MaxLength(CategoryConst.ICON_MAX_LENGTH);
            validator.RuleFor(x => x.Style)!.NotVietnamese().MaxLength(CategoryConst.STYLE_MAX_LENGTH);
            validator.RuleFor(x => x.Views).GreaterThanOrEqual(0);
            validator.Validate();
        }
    }
}