using Commerce.Query.Domain.Constants.Product;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Persistence.Configurations.Product
{
    /// <summary>
    /// EF Core configuration for Product entity
    /// </summary>
    public class ProductConfig : IEntityTypeConfiguration<Entities.Product>
    {
        public void Configure(EntityTypeBuilder<Entities.Product> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName(ProductConst.FIELD_PRODUCT_ID)
                .IsRequired();

            // Thuộc tính Name
            builder.Property(x => x.Name)
                .HasColumnName(ProductConst.FIELD_PRODUCT_NAME)
                .HasMaxLength(ProductConst.PRODUCT_NAME_MAX_LENGTH)
                .IsRequired();
            builder.Property(x => x.Views)
                .HasColumnName(ProductConst.FIELD_PRODUCT_VIEW);

            // Thuộc tính Description
            builder.Property(x => x.Description)
                .HasColumnName(ProductConst.FIELD_PRODUCT_DESCRIPTION)
                .HasMaxLength(ProductConst.PRODUCT_DESCRIPTION_MAX_LENGTH);

            // Thuộc tính Price
            builder.Property(x => x.Price)
                .HasColumnName(ProductConst.FIELD_PRODUCT_PRICE)
                .HasPrecision(ProductConst.PRODUCT_PRICE_MAX_DIGITS, ProductConst.PRODUCT_PRICE_DECIMALS)
                .IsRequired();

            // Thuộc tính CategoryId
            builder.Property(x => x.CategoryId)
                .HasColumnName(ProductConst.FIELD_PRODUCT_CATEGORY_ID)
                .IsRequired();

            // Thuộc tính BrandId
            builder.Property(x => x.BrandId)
                .HasColumnName(ProductConst.FIELD_PRODUCT_BRAND_ID)
                .IsRequired();

            // Thuộc tính ShopId
            builder.Property(x => x.ShopId)
                .HasColumnName(ProductConst.FIELD_PRODUCT_SHOP_ID)
                .IsRequired();

            // Thuộc tính Image
            builder.Property(x => x.Image)
                .HasColumnName(ProductConst.FIELD_PRODUCT_IMAGE)
                .HasMaxLength(ProductConst.PRODUCT_IMAGE_MAX_LENGTH);

            // Thuộc tính InsertedAt
            builder.Property(x => x.InsertedAt)
                .HasColumnName(ProductConst.FIELD_PRODUCT_INSERTED_AT)
                .IsRequired();

            // Thuộc tính InsertedBy
            builder.Property(x => x.InsertedBy)
                .HasColumnName(ProductConst.FIELD_PRODUCT_INSERTED_BY)
                .IsRequired();

            // Thuộc tính UpdatedAt
            builder.Property(x => x.UpdatedAt)
                .HasColumnName(ProductConst.FIELD_PRODUCT_UPDATED_AT);

            // Thuộc tính UpdatedBy
            builder.Property(x => x.UpdatedBy)
                .HasColumnName(ProductConst.FIELD_PRODUCT_UPDATED_BY);

            // Thuộc tính IsDeleted
            builder.Property(x => x.IsDeleted)
                .HasColumnName(ProductConst.FIELD_PRODUCT_IS_DELETED)
                .IsRequired();

            // Thiết lập tên bảng
            builder.ToTable(ProductConst.TABLE_PRODUCT);
            builder.HasMany(o => o.ProductDetails)
                   .WithOne(oi => oi.Product)
                   .HasForeignKey(oi => oi.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
