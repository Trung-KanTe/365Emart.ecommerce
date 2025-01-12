using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;
using Commerce.Query.Domain.Constants.Partner;
using Commerce.Query.Domain.Constants.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Commerce.Query.Domain.Entities.Payment
{
    /// <summary>
    /// Represents a Payment record in the system.
    /// </summary>
    public class Payment : AggregateRoot<Guid>
    {
        /// <summary>
        /// Order ID associated with the Payment.
        /// </summary>
        public Guid? OrderId { get; set; }

        /// <summary>
        /// Amount paid in the Payment.
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Transaction ID for the Payment.
        /// </summary>
        public string? TransactionId { get; set; }

        /// <summary>
        /// Payment method used for the Payment.
        /// </summary>
        public string? PaymentMethod { get; set; }

        // <summary>
        /// Payment method used for the Payment.
        /// </summary>
        public string? ReturnUrl { get; set; }

        // <summary>
        /// Payment method used for the Payment.
        /// </summary>
        public string? OrderInfo { get; set; }

        // <summary>
        /// Payment method used for the Payment.
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Date when the Payment was made.
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Date when the Payment was made.
        /// </summary>
        public string? ResponseCode { get; set; }

        /// <summary>
        /// Status of the Payment.
        /// </summary>
        public string? PaymentStatus { get; set; }

        /// <summary>
        /// Date when the Payment was created.
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// User ID of the person who created this Payment.
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Date when the Payment was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User ID of the person who last updated this Payment.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Indicates whether the Payment is deleted (soft delete).
        /// </summary>
        public bool? IsDeleted { get; set; } = true;

        /// <summary>
        /// Collection of payment details associated with this Payment.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public ICollection<PaymentDetails>? PaymentDetails { get; set; }

        public override void Validate()
        {
        }
    }
}
