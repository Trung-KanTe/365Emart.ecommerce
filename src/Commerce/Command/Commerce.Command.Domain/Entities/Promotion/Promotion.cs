using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.Product;
using Commerce.Command.Domain.Constants.Promotion;

namespace Commerce.Command.Domain.Entities.Promotion
{
    /// <summary>
    /// Promotion entity representing promotional campaigns or discounts
    /// </summary>
    public class Promotion : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the Promotion
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description of the Promotion
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Discount type of the Promotion (e.g., Percentage, Fixed Amount)
        /// </summary>
        public string? DiscountCode { get; set; }

        /// <summary>
        /// Discount value of the Promotion
        /// </summary>
        public decimal DiscountValue { get; set; }

        /// <summary>
        /// Start date of the Promotion
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// End date of the Promotion
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Date when the Promotion was inserted
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// User ID of the person who created this Promotion
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Date when the Promotion was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User ID of the person who last updated this Promotion
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Indicates if the Promotion is deleted (soft delete)
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
            validator.RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(PromotionConst.NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Description)!.MaxLength(PromotionConst.DESCRIPTION_MAX_LENGTH);
            validator.RuleFor(x => x.DiscountCode).NotNull()!.NotEmpty().MaxLength(PromotionConst.DISCOUNT_TYPE_MAX_LENGTH).NotVietnamese();
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing Brand
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(PromotionConst.NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Description)!.MaxLength(PromotionConst.DESCRIPTION_MAX_LENGTH);
            validator.RuleFor(x => x.DiscountCode).NotNull()!.NotEmpty().MaxLength(PromotionConst.DISCOUNT_TYPE_MAX_LENGTH).NotVietnamese();
            validator.Validate();
        }
    }
}
