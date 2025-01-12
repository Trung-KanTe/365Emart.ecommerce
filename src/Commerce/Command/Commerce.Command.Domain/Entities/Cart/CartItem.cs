using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Command.Domain.Entities.Cart
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
