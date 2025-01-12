using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Query.Domain.Entities.Cart
{
    /// <summary>
    /// Represents a shopping cart in the system.
    /// </summary>
    public class Cart : AggregateRoot<Guid>
    {
        /// <summary>
        /// User ID associated with the Cart.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Total quantity of items in the Cart.
        /// </summary>
        public int? TotalQuantity { get; set; } = 0;

        /// <summary>
        /// Date when the Cart was created.
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// User ID of the person who created this Cart.
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Date when the Cart was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User ID of the person who last updated this Cart.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Indicates whether the Cart is deleted (soft delete).
        /// </summary>
        public bool? IsDeleted { get; set; } = true;

        /// <summary>
        /// Collection of items associated with the Cart.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public ICollection<CartItem>? CartItems { get; set; }

        public override void Validate()
        {
        }
    }
}
