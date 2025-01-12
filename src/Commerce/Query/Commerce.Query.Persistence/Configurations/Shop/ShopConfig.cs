using Commerce.Query.Domain.Constants.Shop;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities = Commerce.Query.Domain.Entities.Shop;

namespace Commerce.Query.Persistence.Configurations.Shop
{
    /// <summary>
    /// EF Core configuration for Shop entity
    /// </summary>
    public class ShopConfig : IEntityTypeConfiguration<Entities.Shop>
    {
        public void Configure(EntityTypeBuilder<Entities.Shop> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(ShopConst.FIELD_SHOP_ID);

            builder.Property(x => x.Name)
                .HasColumnName(ShopConst.FIELD_SHOP_NAME)
                .HasMaxLength(ShopConst.SHOP_NAME_MAX_LENGTH)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName(ShopConst.FIELD_SHOP_DESCRIPTION)
                .HasMaxLength(ShopConst.SHOP_DESCRIPTION_MAX_LENGTH);

            builder.Property(x => x.Image)
                .HasColumnName(ShopConst.FIELD_SHOP_IMAGE)
                .HasMaxLength(ShopConst.SHOP_IMAGE_MAX_LENGTH);

            builder.Property(x => x.Style)
                .HasColumnName(ShopConst.FIELD_SHOP_STYLE)
                .HasMaxLength(ShopConst.SHOP_STYLE_MAX_LENGTH);

            builder.Property(x => x.Tel)
                .HasColumnName(ShopConst.FIELD_SHOP_TEL)
                .HasMaxLength(ShopConst.SHOP_TEL_MAX_LENGTH);

            builder.Property(x => x.Email)
                .HasColumnName(ShopConst.FIELD_SHOP_EMAIL)
                .HasMaxLength(ShopConst.SHOP_EMAIL_MAX_LENGTH);

            builder.Property(x => x.Website)
                .HasColumnName(ShopConst.FIELD_SHOP_WEBSITE)
                .HasMaxLength(ShopConst.SHOP_WEBSITE_MAX_LENGTH);

            builder.Property(x => x.Address)
                .HasColumnName(ShopConst.FIELD_SHOP_ADDRESS)
                .HasMaxLength(ShopConst.SHOP_ADDRESS_MAX_LENGTH);

            builder.Property(x => x.WardId)
                .HasColumnName(ShopConst.FIELD_SHOP_WARD_ID);

            builder.Property(x => x.UserId)
                .HasColumnName(ShopConst.FIELD_SHOP_USER_ID);

            builder.Property(x => x.PartnerId)
                .HasColumnName(ShopConst.FIELD_SHOP_PARTNER_ID);

            builder.Property(x => x.Views)
                .HasColumnName(ShopConst.FIELD_SHOP_VIEWS);

            builder.Property(x => x.InsertedAt)
                .HasColumnName(ShopConst.FIELD_SHOP_INSERTED_AT);

            builder.Property(x => x.InsertedBy)
                .HasColumnName(ShopConst.FIELD_SHOP_INSERTED_BY);

            builder.Property(x => x.UpdatedAt)
                .HasColumnName(ShopConst.FIELD_SHOP_UPDATED_AT);

            builder.Property(x => x.UpdatedBy)
                .HasColumnName(ShopConst.FIELD_SHOP_UPDATED_BY);

            builder.Property(x => x.IsDeleted)
                .HasColumnName(ShopConst.FIELD_SHOP_IS_DELETED);

            builder.ToTable(ShopConst.TABLE_SHOP);
        }
    }
}
