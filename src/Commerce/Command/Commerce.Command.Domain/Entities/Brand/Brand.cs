using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.Brand;

namespace Commerce.Command.Domain.Entities.Brand
{
    /// <summary>
    /// Domain entity with guid key type
    /// </summary>
    public class Brand : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of Brand
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Icon of Brand
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// Style of Brand
        /// </summary>
        public string? Style { get; set; }

        /// <summary>
        /// User Id of Brand
        /// </summary>
        public Guid? UserId { get; set; }


        /// <summary>
        /// Views of Brand
        /// </summary>
        public int? Views { get; set; } = 0;

        /// <summary>
        /// Inserted date of the Brand
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// Inserted User of the Brand
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Update date of the Brand
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Updated User of the Brand
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Deleted status of the Brand
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
        /// Validate when creating a new Brand
        /// </summary>
        public void ValidateCreate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.Name).NotNull().NotEmpty().MaxLength(BrandConst.NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Icon)!.MaxLength(BrandConst.ICON_MAX_LENGTH).NotVietnamese() ;
            validator.RuleFor(x => x.Style)!.MaxLength(BrandConst.STYLE_MAX_LENGTH).NotVietnamese();
            validator.RuleFor(x => x.UserId).NotNull().IsGuid();
            validator.RuleFor(x => x.Views).GreaterThanOrEqual(0);
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing Brand
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.Name).NotNull().NotEmpty().MaxLength(BrandConst.NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Icon)!.MaxLength(BrandConst.ICON_MAX_LENGTH).NotVietnamese();
            validator.RuleFor(x => x.Style)!.MaxLength(BrandConst.STYLE_MAX_LENGTH).NotVietnamese();
            validator.RuleFor(x => x.UserId).IsGuid();
            validator.RuleFor(x => x.Views).GreaterThanOrEqual(0);
            validator.Validate();
        }
    }
}