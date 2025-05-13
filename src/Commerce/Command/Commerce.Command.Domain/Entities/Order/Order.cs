using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.Order;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Command.Domain.Entities.Order
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
        public string? Address { get; set; }

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
            validator.RuleFor(x => x.UserId).NotNull().IsGuid();
            validator.RuleFor(x => x.PromotionId).IsGuid();          
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing Brand
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.UserId).IsGuid();
            validator.RuleFor(x => x.PromotionId).IsGuid();
            validator.RuleFor(x => x.PaymentMethod)!.NotEmpty().MaxLength(OrderConst.ORDER_PAYMENT_METHOD_MAX_LENGTH);
            validator.RuleFor(x => x.Status)!.NotEmpty().MaxLength(OrderConst.ORDER_STATUS_MAX_LENGTH);
            validator.Validate();
        }
    }
}
