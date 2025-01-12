using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Aggregates;
using Commerce.Command.Domain.Constants.Partner;

namespace Commerce.Command.Domain.Entities.Partner
{
    /// <summary>
    /// Domain entity with guid key type
    /// </summary>
    public class Partner : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the User
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Email of the User
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Hashed Password of the User
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// Phone number of the User
        /// </summary>
        public string? Tel { get; set; }

        /// <summary>
        /// Phone number of the User
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Phone number of the User
        /// </summary>
        public string? Website { get; set; }

        /// <summary>
        /// Address of the User
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Ward ID for the User's location
        /// </summary>
        public Guid? WardId { get; set; }

        /// <summary>
        /// Date when the User was created
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// User ID of the person who created this User
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Date when the User was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User ID of the person who last updated this User
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Status indicating if the User is deleted (soft delete)
        /// </summary>
        public bool? IsDeleted { get; set; } = true;
    

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
            validator.RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(PartnerConst.PARTNER_NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Description)!.MaxLength(PartnerConst.PARTNER_DESCIPTION_MAX_LENGTH);
            validator.RuleFor(x => x.Icon)!.MaxLength(PartnerConst.PARTNER_ICON_MAX_LENGTH).NotVietnamese();
            validator.RuleFor(x => x.Tel)!.MaxLength(PartnerConst.PARTNER_TEL_MAX_LENGTH).IsPhoneNumber();
            validator.RuleFor(x => x.Email)!.MaxLength(PartnerConst.PARTNER_EMAIL_MAX_LENGTH).IsEmail();
            validator.RuleFor(x => x.Website)!.MaxLength(PartnerConst.PARTNER_WEBSITE_MAX_LENGTH);
            validator.RuleFor(x => x.Address)!.MaxLength(PartnerConst.PARTNER_ADDRESS_MAX_LENGTH);
            validator.RuleFor(x => x.WardId).NotNull().IsGuid();
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing Brand
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.RuleFor(x => x.Name)!.NotEmpty().MaxLength(PartnerConst.PARTNER_NAME_MAX_LENGTH);
            validator.RuleFor(x => x.Description)!.MaxLength(PartnerConst.PARTNER_DESCIPTION_MAX_LENGTH);
            validator.RuleFor(x => x.Icon)!.MaxLength(PartnerConst.PARTNER_ICON_MAX_LENGTH).NotVietnamese();
            validator.RuleFor(x => x.Tel)!.MaxLength(PartnerConst.PARTNER_TEL_MAX_LENGTH).IsPhoneNumber();
            validator.RuleFor(x => x.Email)!.MaxLength(PartnerConst.PARTNER_EMAIL_MAX_LENGTH).IsEmail();
            validator.RuleFor(x => x.Website)!.MaxLength(PartnerConst.PARTNER_WEBSITE_MAX_LENGTH);
            validator.RuleFor(x => x.Address)!.MaxLength(PartnerConst.PARTNER_ADDRESS_MAX_LENGTH);
            validator.RuleFor(x => x.WardId).IsGuid();
            validator.Validate();
        }
    }
}