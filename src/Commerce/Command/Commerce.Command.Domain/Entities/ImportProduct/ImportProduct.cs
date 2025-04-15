using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.Category;
using Commerce.Command.Domain.Constants.ImportProduct;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Command.Domain.Entities.ImportProduct
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

        /// <summary>
        /// Warehouse ID where the product is stored.
        /// </summary>
        public Guid? WareHouseId { get; set; }

        public Guid? ShopId { get; set; }

        /// <summary>
        /// Date of import.
        /// </summary>
        public DateTime? ImportDate { get; set; } = DateTime.UtcNow;

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
            validator.RuleFor(x => x.PartnerId).NotNull().IsGuid();
            validator.RuleFor(x => x.WareHouseId).NotNull().IsGuid();
            validator.RuleFor(x => x.Note)!.MaxLength(ImportProductConst.IMPORT_PRODUCT_NOTE_MAX_LENGTH);
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing User
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.PartnerId).IsGuid();
            validator.RuleFor(x => x.WareHouseId).IsGuid();
            validator.RuleFor(x => x.Note)!.MaxLength(ImportProductConst.IMPORT_PRODUCT_NOTE_MAX_LENGTH);
            validator.Validate();
        }
    }
}
