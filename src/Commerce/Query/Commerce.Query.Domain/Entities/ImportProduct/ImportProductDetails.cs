using Commerce.Query.Domain.Abstractions.Aggregates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Query.Domain.Entities.ImportProduct
{
    /// <summary>
    /// Represents the details of an Import Product.
    /// </summary>
    public class ImportProductDetails : AggregateRoot<Guid>
    {
        /// <summary>
        /// Product ID associated with the detail.
        /// </summary>
        public Guid? ProductId { get; set; }

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

        public override void Validate()
        {
        }
    }
}
