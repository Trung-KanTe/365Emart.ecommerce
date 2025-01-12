using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;
using Commerce.Query.Domain.Constants.Product;
using Commerce.Query.Domain.Constants.Promotion;

namespace Commerce.Query.Domain.Entities.Promotion
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
        public string? DiscountType { get; set; }

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

        public override void Validate()
        {
        }
    }
}
