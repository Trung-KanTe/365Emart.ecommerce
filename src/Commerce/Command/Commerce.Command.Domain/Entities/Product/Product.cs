using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.Partner;
using Commerce.Command.Domain.Constants.Product;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Command.Domain.Entities.Product
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
            validator.RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(ProductConst.PRODUCT_NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Description)!.MaxLength(ProductConst.PRODUCT_DESCRIPTION_MAX_LENGTH);
            validator.RuleFor(x => x.CategoryId).NotNull().IsGuid();
            validator.RuleFor(x => x.BrandId).NotNull().IsGuid();
            validator.RuleFor(x => x.ShopId).NotNull().IsGuid();
            validator.RuleFor(x => x.Image)!.MaxLength(ProductConst.PRODUCT_IMAGE_MAX_LENGTH);
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing Brand
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.Name)!.NotEmpty().MaxLength(ProductConst.PRODUCT_NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Description)!.MaxLength(ProductConst.PRODUCT_DESCRIPTION_MAX_LENGTH);
            validator.RuleFor(x => x.CategoryId).IsGuid();
            validator.RuleFor(x => x.BrandId).IsGuid();
            validator.RuleFor(x => x.ShopId).IsGuid();
            validator.RuleFor(x => x.Image)!.MaxLength(ProductConst.PRODUCT_IMAGE_MAX_LENGTH);
            validator.Validate();
        }
    }
}
