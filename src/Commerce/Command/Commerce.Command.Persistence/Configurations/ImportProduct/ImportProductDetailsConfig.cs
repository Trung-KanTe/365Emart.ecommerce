using Commerce.Command.Domain.Constants.ImportProduct;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Command.Domain.Entities.ImportProduct;

namespace Commerce.Command.Persistence.Configurations.ImportProduct
{
    /// <summary>
    /// EF Core configuration for ImportProductDetails entity
    /// </summary>
    public class ImportProductDetailsConfig : IEntityTypeConfiguration<Entities.ImportProductDetails>
    {
        public void Configure(EntityTypeBuilder<Entities.ImportProductDetails> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_DETAILS_ID);

            builder.Property(x => x.ImportProductId)
                .HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_DETAILS_IMPORT_PRODUCT_ID);

            builder.Property(x => x.ProductId)
                .HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_DETAILS_PRODUCT_ID)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_DETAILS_QUANTITY)
                .IsRequired();

            builder.Property(x => x.ImportPrice)
                .HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_DETAILS_IMPORT_PRICE)
                .IsRequired();

            builder.ToTable(ImportProductConst.TABLE_IMPORT_PRODUCT_DETAILS);
            builder.HasOne(mer => mer.ImportProduct).WithMany(ver => ver.ImportProductDetails).HasForeignKey(mer => mer.ImportProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
