using Commerce.Query.Contract.Contants;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;
using Commerce.Query.Domain.Constants.Category;
using Commerce.Query.Domain.Constants.ImportProduct;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Query.Domain.Entities.ImportProduct
{
    /// <summary>
    /// Represents an Import Product record in the system.
    /// </summary>
    public class ImportProduct : AggregateRoot<Guid>
    {
        /// <summary>
        /// Partner ID associated with the Import Product.
        /// </summary>
        public Guid? PartnerId { get; set; }

        public Guid? ShopId { get; set; }

        /// <summary>
        /// Warehouse ID where the product is stored.
        /// </summary>
        public Guid? WareHouseId { get; set; }

        /// <summary>
        /// Date of import.
        /// </summary>
        public DateTime? ImportDate { get; set; } 

        /// <summary>
        /// Note for the Import Product.
        /// </summary>
        public string? Note { get; set; }

        /// <summary>
        /// Date when the Import Product was created.
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// User ID of the person who created this Import Product.
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Date when the Import Product was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User ID of the person who last updated this Import Product.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Indicates whether the Import Product is deleted (soft delete).
        /// </summary>
        public bool? IsDeleted { get; set; } = true;

        /// <summary>
        /// Collection of details associated with the Import Product.
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public ICollection<ImportProductDetails>? ImportProductDetails { get; set; }

        public override void Validate()
        {
        }
    }
}
