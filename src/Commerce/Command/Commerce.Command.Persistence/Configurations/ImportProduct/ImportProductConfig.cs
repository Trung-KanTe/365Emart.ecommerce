using Commerce.Command.Domain.Constants.ImportProduct;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Command.Domain.Entities.ImportProduct;

namespace Commerce.Command.Persistence.Configurations.ImportProduct
{
    /// <summary>
    /// EF Core configuration for ImportProduct entity
    /// </summary>
    public class ImportProductConfig : IEntityTypeConfiguration<Entities.ImportProduct>
    {
        public void Configure(EntityTypeBuilder<Entities.ImportProduct> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_ID);

            builder.Property(x => x.PartnerId)
                .HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_PARTNER_ID)
                .IsRequired();

            builder.Property(x => x.WareHouseId)
                .HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_WAREHOUSE_ID);

            builder.Property(x => x.ImportDate)
                .HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_IMPORT_DATE);

            builder.Property(x => x.Note)
                .HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_NOTE)
                .HasMaxLength(ImportProductConst.IMPORT_PRODUCT_NOTE_MAX_LENGTH);

            builder.Property(x => x.InsertedAt)
                .HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_INSERTED_AT);

            builder.Property(x => x.InsertedBy)
                .HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_INSERTED_BY);

            builder.Property(x => x.UpdatedAt)
                .HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_UPDATED_AT);

            builder.Property(x => x.UpdatedBy)
                .HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_UPDATED_BY);

            builder.Property(x => x.IsDeleted)
                .HasColumnName(ImportProductConst.FIELD_IMPORT_PRODUCT_IS_DELETED);

            builder.ToTable(ImportProductConst.TABLE_IMPORT_PRODUCT);
            builder.HasMany(c => c.ImportProductDetails).WithOne(verify => verify.ImportProduct).HasForeignKey(mer => mer.ImportProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
