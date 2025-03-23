using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.Category;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Command.Domain.Entities.ImportProduct
{
    /// <summary>
    /// Represents the details of an Import Product.
    /// </summary>
    public class ImportProductDetails : AggregateRoot<Guid>
    {
        /// <summary>
        /// Product ID associated with the detail.
        /// </summary>
        public Guid? ProductDetailId { get; set; }

        /// <summary>
        /// ID of the Import Product this detail belongs to.
        /// </summary>
        public Guid? ImportProductId { get; set; }

        /// <summary>
        /// Import price for the product.
        /// </summary>
        public decimal? ImportPrice { get; set; }

        /// <summary>
        /// Quantity of the product imported.
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        /// Navigation property to the Import Product.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public ImportProduct? ImportProduct { get; set; }

        /// <summary>
        /// Implement the Validate method from Entity<Guid>
        /// </summary>
        public override void Validate()
        {
            ValidateCreate();
            ValidateUpdate();
        }

        /// <summary>
        /// Validate when creating a new User
        /// </summary>
        public void ValidateCreate()
        {
            var validator = Validator.Create(this);         
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing User
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);       
            validator.Validate();
        }
    }
}
