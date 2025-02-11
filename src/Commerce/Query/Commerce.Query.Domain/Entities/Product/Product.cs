using Commerce.Query.Domain.Abstractions.Aggregates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Query.Domain.Entities.Product
{
    public class Product : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the Product
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description of the Product
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Description of the Product
        /// </summary>
        public int? Views { get; set; } = 0;

        /// <summary>
        /// Price of the Product
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Category ID associated with the Product
        /// </summary>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Brand ID associated with the Product
        /// </summary>
        public Guid? BrandId { get; set; }

        /// <summary>
        /// Shop ID associated with the Product
        /// </summary>
        public Guid? ShopId { get; set; }

        /// <summary>
        /// Image of the Product
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Date when the Product was created
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// User ID of the person who created this Product
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Date when the Product was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User ID of the person who last updated this Product
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Status indicating if the Product is deleted (soft delete)
        /// </summary>
        public bool? IsDeleted { get; set; } = true;

        [NotMapped]
        [JsonIgnore]
        public ICollection<ProductDetail>? ProductDetails { get; set; }

        public override void Validate()
        {
        }
    }
}
