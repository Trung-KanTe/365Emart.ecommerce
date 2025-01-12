using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Command.Domain.Entities.Order
{
    /// <summary>
    /// Represents an OrderItem record in the system.
    /// </summary>
    public class OrderItem : AggregateRoot<Guid>
    {
        /// <summary>
        /// Order ID associated with the OrderItem.
        /// </summary>
        public Guid? OrderId { get; set; }

        /// <summary>
        /// Product ID associated with the OrderItem.
        /// </summary>
        public Guid? ProductId { get; set; }

        /// <summary>
        /// Quantity of the product in the OrderItem.
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        /// Price of the product in the OrderItem.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Total amount for the OrderItem (Quantity * Price).
        /// </summary>
        public decimal? Total { get; set; }

        /// <summary>
        /// Navigation property to the Order this item belongs to.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public Order? Order { get; set; }

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
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing Brand
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);      
            validator.Validate();
        }
    }
}