using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.Category;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Command.Domain.Entities.Category
{
    /// <summary>
    /// Domain entity with int key type
    /// </summary>
    public class Category : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of category
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Image of category
        /// </summary>
        public string? Image {  get; set; }

        /// <summary>
        /// Style of category
        /// </summary>
        public string? Style { get; set; }

        /// <summary>
        /// User Id of category
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Views of category
        /// </summary>
        public int? Views {  get; set; }

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
        public ICollection<ClassificationCategory>? ClassificationCategories { get; set; }

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
            validator.RuleFor(x => x.Image)!.NotVietnamese().MaxLength(CategoryConst.IMAGE_MAX_LENGTH);
            validator.RuleFor(x => x.Style)!.NotVietnamese().MaxLength(CategoryConst.STYLE_MAX_LENGTH);
            validator.RuleFor(x => x.UserId).NotNull().IsGuid();
            validator.RuleFor(x => x.Views).GreaterThanOrEqual(0);
            validator.RuleFor(x => x.ClassificationCategories).Must(ids => ids != null && ids.All(id => id.ClassificationId != Guid.Empty), ErrCodeConst.CONFLICT);
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing User
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.Name)!.MaxLength(CategoryConst.NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Image)!.NotVietnamese().MaxLength(CategoryConst.IMAGE_MAX_LENGTH);
            validator.RuleFor(x => x.Style)!.NotVietnamese().MaxLength(CategoryConst.STYLE_MAX_LENGTH);
            validator.RuleFor(x => x.UserId).IsGuid();
            validator.RuleFor(x => x.Views).GreaterThanOrEqual(0);
            validator.RuleFor(x => x.ClassificationCategories).Must(ids => ids != null && ids.All(id => id.ClassificationId != Guid.Empty), ErrCodeConst.CONFLICT);
            validator.Validate();
        }
    }
}