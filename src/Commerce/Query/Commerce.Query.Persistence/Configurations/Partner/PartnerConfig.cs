using Commerce.Query.Domain.Constants.Partner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Query.Domain.Entities.Partner;

namespace Commerce.Query.Persistence.Configurations.Partner
{
    /// <summary>
    /// EF Core configuration for Partner entity
    /// </summary>
    public class PartnerConfig : IEntityTypeConfiguration<Entities.Partner>
    {
        public void Configure(EntityTypeBuilder<Entities.Partner> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(PartnerConst.FIELD_PARTNER_ID);
            builder.Property(x => x.Name).HasColumnName(PartnerConst.FIELD_PARTNER_NAME).HasMaxLength(PartnerConst.PARTNER_NAME_MAX_LENGTH).IsRequired();
            builder.Property(x => x.Description).HasColumnName(PartnerConst.FIELD_PARTNER_DESCIPTION).HasMaxLength(PartnerConst.PARTNER_DESCIPTION_MAX_LENGTH);
            builder.Property(x => x.Icon).HasColumnName(PartnerConst.FIELD_PARTNER_ICON).HasMaxLength(PartnerConst.PARTNER_ICON_MAX_LENGTH);
            builder.Property(x => x.Tel).HasColumnName(PartnerConst.FIELD_PARTNER_TEL).HasMaxLength(PartnerConst.PARTNER_TEL_MAX_LENGTH);
            builder.Property(x => x.Email).HasColumnName(PartnerConst.FIELD_PARTNER_EMAIL).HasMaxLength(PartnerConst.PARTNER_EMAIL_MAX_LENGTH);
            builder.Property(x => x.Website).HasColumnName(PartnerConst.FIELD_PARTNER_WEBSITE).HasMaxLength(PartnerConst.PARTNER_WEBSITE_MAX_LENGTH);
            builder.Property(x => x.Address).HasColumnName(PartnerConst.FIELD_PARTNER_ADDRESS).HasMaxLength(PartnerConst.PARTNER_ADDRESS_MAX_LENGTH);
            builder.Property(x => x.WardId).HasColumnName(PartnerConst.FIELD_PARTNER_WARD_ID);
            builder.Property(x => x.InsertedAt).HasColumnName(PartnerConst.FIELD_PARTNER_INSERTED_AT);
            builder.Property(x => x.InsertedBy).HasColumnName(PartnerConst.FIELD_PARTNER_INSERTED_BY);
            builder.Property(x => x.UpdatedAt).HasColumnName(PartnerConst.FIELD_PARTNER_UPDATED_AT);
            builder.Property(x => x.UpdatedBy).HasColumnName(PartnerConst.FIELD_PARTNER_UPDATED_BY);
            builder.Property(x => x.IsDeleted).HasColumnName(PartnerConst.FIELD_PARTNER_IS_DELETED);
            builder.ToTable(PartnerConst.TABLE_PARTNER);
        }
    }
}