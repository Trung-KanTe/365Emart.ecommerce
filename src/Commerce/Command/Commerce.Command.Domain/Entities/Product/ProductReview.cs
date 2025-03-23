using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.Product;

namespace Commerce.Command.Domain.Entities.Product
{
    /// <summary>
    /// Represents a review of a product by a user.
    /// </summary>
    public class ProductReview : AggregateRoot<Guid>
    {
        /// <summary>
        /// The unique identifier of the product being reviewed.
        /// </summary>
        public Guid? ProductId { get; set; }

        /// <summary>
        /// The unique identifier of the user who created the review.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// The rating given to the product, ranging from 1 to 5.
        /// </summary>
        public int? Rating { get; set; } = 1;

        /// <summary>
        /// The comment provided by the user for the product review.
        /// </summary>
        public string? Comment { get; set; }
        public string? Image { get; set; }

        /// <summary>
        /// The date and time when the review was created.
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// The unique identifier of the user who created the review.
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// The date and time when the review was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// The unique identifier of the user who last updated the review.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Indicates whether the review has been soft-deleted.
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
            validator.RuleFor(x => x.ProductId).NotNull().IsGuid();
            validator.RuleFor(x => x.UserId).NotNull().IsGuid();
            validator.RuleFor(x => x.Rating).GreaterThanOrEqual(0).LowerThanOrEqual(5);
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing Brand
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.ProductId).NotNull().IsGuid();
            validator.RuleFor(x => x.UserId).NotNull().IsGuid();
            validator.RuleFor(x => x.Rating).GreaterThanOrEqual(0).LowerThanOrEqual(5);
            validator.Validate();
        }
    }
}
