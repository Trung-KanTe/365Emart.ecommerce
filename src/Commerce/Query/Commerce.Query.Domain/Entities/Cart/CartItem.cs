using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Query.Domain.Entities.Cart
{
    /// <summary>
    /// Represents an item in a shopping cart.
    /// </summary>
    public class CartItem : AggregateRoot<Guid>
    {
        /// <summary>
        /// ID of the Cart this item belongs to.
        /// </summary>
        public Guid? CartId { get; set; }

        /// <summary>
        /// ID of the Product associated with this item.
        /// </summary>
        public Guid? ProductId { get; set; }

        /// <summary>
        /// Price of the product when added to the cart.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Quantity of the product in the cart.
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        /// Total price for this cart item (Price * Quantity).
        /// </summary>
        public decimal? Total { get; set; }

        /// <summary>
        /// Navigation property to the Import Product.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public Cart? Cart { get; set; }

        public override void Validate()
        {
        }
    }
}
