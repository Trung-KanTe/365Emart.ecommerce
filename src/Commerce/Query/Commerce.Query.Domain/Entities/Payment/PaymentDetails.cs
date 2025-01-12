using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Query.Domain.Entities.Payment
{
    /// <summary>
    /// Represents details of a Payment record in the system.
    /// </summary>
    public class PaymentDetails : AggregateRoot<Guid>
    {
        /// <summary>
        /// Payment ID associated with these details.
        /// </summary>
        public Guid? PaymentId { get; set; }

        /// <summary>
        /// Transaction code for the payment.
        /// </summary>
        public string? BankCode { get; set; }

        /// <summary>
        /// Bank name associated with the payment.
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// Card number used in the payment.
        /// </summary>
        public string? CardNumber { get; set; }

        /// <summary>
        /// Card number used in the payment.
        /// </summary>
        public string? ExtraData { get; set; }

        /// <summary>
        /// Note for additional information about the payment.
        /// </summary>
        public string? Note { get; set; }

        /// <summary>
        /// Navigation property to the Payment this detail belongs to.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public Payment? Payment { get; set; }

        public override void Validate()
        {
        }
    }
}
