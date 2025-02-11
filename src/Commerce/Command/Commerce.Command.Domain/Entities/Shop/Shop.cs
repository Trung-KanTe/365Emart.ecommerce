using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.Brand;
using Commerce.Command.Domain.Constants.Shop;

namespace Commerce.Command.Domain.Entities.Shop
{
    public class Shop : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the Shop
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description of the Shop
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Image of the Shop
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Style of the Shop
        /// </summary>
        public string? Style { get; set; }

        /// <summary>
        /// Phone number of the Shop
        /// </summary>
        public string? Tel { get; set; }

        /// <summary>
        /// Email of the Shop
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Website of the Shop
        /// </summary>
        public string? Website { get; set; }

        /// <summary>
        /// Address of the Shop
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Ward ID for the Shop's location
        /// </summary>
        public Guid? WardId { get; set; }

        /// <summary>
        /// User ID associated with the Shop
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Number of views for the Shop
        /// </summary>
        public int? Views { get; set; } = 0;

        /// <summary>
        /// Date when the Shop was created
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// User ID of the person who created this Shop
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Date when the Shop was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User ID of the person who last updated this Shop
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Status indicating if the Shop is deleted (soft delete)
        /// </summary>
        public bool? IsDeleted { get; set; } = false;

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
            validator.RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(ShopConst.SHOP_NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Description)!.MaxLength(ShopConst.SHOP_DESCRIPTION_MAX_LENGTH);
            validator.RuleFor(x => x.Image)!.MaxLength(ShopConst.SHOP_IMAGE_MAX_LENGTH).NotVietnamese();
            validator.RuleFor(x => x.Style)!.MaxLength(ShopConst.SHOP_STYLE_MAX_LENGTH);
            validator.RuleFor(x => x.Tel)!.MaxLength(ShopConst.SHOP_TEL_MAX_LENGTH).IsPhoneNumber();
            validator.RuleFor(x => x.Email)!.MaxLength(ShopConst.SHOP_EMAIL_MAX_LENGTH).IsEmail();
            validator.RuleFor(x => x.Website)!.MaxLength(ShopConst.SHOP_WEBSITE_MAX_LENGTH);
            validator.RuleFor(x => x.Address)!.MaxLength(ShopConst.SHOP_ADDRESS_MAX_LENGTH);
            validator.RuleFor(x => x.Views)!.GreaterThanOrEqual(0);
            validator.RuleFor(x => x.WardId).NotNull().IsGuid();
            validator.RuleFor(x => x.UserId).NotNull().IsGuid();
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing Brand
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.Name)!.NotEmpty().MaxLength(ShopConst.SHOP_NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Description)!.MaxLength(ShopConst.SHOP_DESCRIPTION_MAX_LENGTH);
            validator.RuleFor(x => x.Image)!.MaxLength(ShopConst.SHOP_IMAGE_MAX_LENGTH).NotVietnamese();
            validator.RuleFor(x => x.Style)!.MaxLength(ShopConst.SHOP_STYLE_MAX_LENGTH);
            validator.RuleFor(x => x.Tel)!.MaxLength(ShopConst.SHOP_TEL_MAX_LENGTH).IsPhoneNumber();
            validator.RuleFor(x => x.Email)!.MaxLength(ShopConst.SHOP_EMAIL_MAX_LENGTH).IsEmail();
            validator.RuleFor(x => x.Website)!.MaxLength(ShopConst.SHOP_WEBSITE_MAX_LENGTH);
            validator.RuleFor(x => x.Address)!.MaxLength(ShopConst.SHOP_ADDRESS_MAX_LENGTH);
            validator.RuleFor(x => x.Views)!.GreaterThanOrEqual(0);
            validator.RuleFor(x => x.WardId).IsGuid();
            validator.RuleFor(x => x.UserId).IsGuid();
            validator.Validate();
        }
    }
}
