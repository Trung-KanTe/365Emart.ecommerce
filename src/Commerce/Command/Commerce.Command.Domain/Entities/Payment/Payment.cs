using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.Partner;
using Commerce.Command.Domain.Constants.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Commerce.Command.Domain.Entities.Payment
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
            validator.RuleFor(x => x.OrderId)
                .NotNull()
                .IsGuid();
            validator.RuleFor(x => x.TransactionId)!
                .MaxLength(PaymentConst.PAYMENT_TRANSACTION_ID_MAX_LENGTH);


            validator.RuleFor(x => x.PaymentMethod)
                .MaxLength(PaymentConst.PAYMENT_METHOD_MAX_LENGTH);


            validator.RuleFor(x => x.PaymentStatus)
                .MaxLength(PaymentConst.PAYMENT_STATUS_MAX_LENGTH);
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing Brand
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.OrderId)
                .NotNull()
                .IsGuid();
            validator.RuleFor(x => x.TransactionId)!
                .MaxLength(PaymentConst.PAYMENT_TRANSACTION_ID_MAX_LENGTH);


            validator.RuleFor(x => x.PaymentMethod)
                .MaxLength(PaymentConst.PAYMENT_METHOD_MAX_LENGTH);


            validator.RuleFor(x => x.PaymentStatus)
                .MaxLength(PaymentConst.PAYMENT_STATUS_MAX_LENGTH);
            validator.Validate();
        }
    }
}
