using Commerce.Command.Domain.Constants.Product;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities = Commerce.Command.Domain.Entities.Product;

namespace Commerce.Command.Persistence.Configurations.Product
{
    /// <summary>
    /// EF Core configuration for Product entity
    /// </summary>
    public class ProductDetailConfig : IEntityTypeConfiguration<Entities.ProductDetail>
    {
        public void Configure(EntityTypeBuilder<Entities.ProductDetail> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName(ProductConst.FIELD_PRODUCT_DETAIL_ID)
                .IsRequired();
            builder.Property(x => x.ProductId)
                .HasColumnName(ProductConst.FIELD_PRODUCT_ID)
                .IsRequired();

            builder.Property(x => x.Size)
                .HasColumnName(ProductConst.FIELD_PRODUCT_SIZE);
            builder.Property(x => x.Color)
                .HasColumnName(ProductConst.FIELD_PRODUCT_COLOR);
            builder.Property(x => x.StockQuantity)
                .HasColumnName(ProductConst.FIELD_PRODUCT_DETAIL_QUANTITY);
           
            // Thiết lập tên bảng
            builder.ToTable(ProductConst.TABLE_PRODUCT_DETAIL);
            builder.HasOne(oi => oi.Product)
                   .WithMany(o => o.ProductDetails)
                   .HasForeignKey(oi => oi.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
