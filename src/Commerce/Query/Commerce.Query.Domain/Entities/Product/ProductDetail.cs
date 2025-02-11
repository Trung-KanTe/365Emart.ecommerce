using Commerce.Query.Domain.Abstractions.Aggregates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Query.Domain.Entities.Product
{
    public class ProductDetail : AggregateRoot<Guid>
    {
        /// <summary>
        /// The unique identifier of the product being reviewed.
        /// </summary>
        public Guid? ProductId { get; set; }

        /// <summary>
        /// Description of the Product
        /// </summary>
        public string? Size { get; set; }

        /// <summary>
        /// Description of the Product
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// Description of the Product
        /// </summary>
        public int? StockQuantity { get; set; }

        [NotMapped]
        [JsonIgnore]
        public Product? Product { get; set; }

        public override void Validate()
        {
        }
    }
}
