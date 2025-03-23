using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;

namespace Commerce.Query.Domain.Entities.Product
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

        public override void Validate()
        {
        }
    }
}
