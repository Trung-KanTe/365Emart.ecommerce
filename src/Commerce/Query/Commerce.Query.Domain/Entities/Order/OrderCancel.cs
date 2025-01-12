using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;
using Commerce.Query.Domain.Constants.Order;

namespace Commerce.Query.Domain.Entities.Order
{
    /// <summary>
    /// Represents an order cancellation.
    /// </summary>
    public class OrderCancel : AggregateRoot<Guid>
    {
        /// <summary>
        /// Order ID associated with the cancellation.
        /// </summary>
        public Guid? OrderId { get; set; }

        /// <summary>
        /// Reason for the cancellation.
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Refund amount for the cancellation.
        /// </summary>
        public decimal? RefundAmount { get; set; }

        /// <summary>
        /// Indicates if the refund has been processed.
        /// </summary>
        public bool IsRefunded { get; set; } = false;

        /// <summary>
        /// Date when the cancellation record was created.
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// User ID of the person who created this cancellation record.
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Date when the cancellation record was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User ID of the person who last updated this cancellation record.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Indicates if the record is deleted (soft delete).
        /// </summary>
        public bool IsDeleted { get; set; } = true;

        public override void Validate()
        {
        }
    }
}
