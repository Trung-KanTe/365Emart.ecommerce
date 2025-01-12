using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;
using Commerce.Query.Domain.Constants.Order;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Query.Domain.Entities.Order
{
    /// <summary>
    /// Represents an Order record in the system.
    /// </summary>
    public class Order : AggregateRoot<Guid>
    {
        /// <summary>
        /// User ID associated with the Order.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Promotion ID associated with the Order.
        /// </summary>
        public Guid? PromotionId { get; set; }

        /// <summary>
        /// Total amount of the Order.
        /// </summary>
        public decimal? TotalAmount { get; set; }

        /// <summary>
        /// Payment method used for the Order.
        /// </summary>
        public string? PaymentMethod { get; set; }

        /// <summary>
        /// Status of the Order.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Date when the Order was created.
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// User ID of the person who created this Order.
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Date when the Order was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User ID of the person who last updated this Order.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Indicates whether the Order is deleted (soft delete).
        /// </summary>
        public bool? IsDeleted { get; set; } = true;

        /// <summary>
        /// Collection of items associated with the Order.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public ICollection<OrderItem>? OrderItems { get; set; }

        public override void Validate()
        {
        }
    }
}
