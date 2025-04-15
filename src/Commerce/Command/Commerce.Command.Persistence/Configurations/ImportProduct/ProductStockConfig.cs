using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities = Commerce.Command.Domain.Entities.ProductStock;
using Commerce.Command.Domain.Constants.ImportProduct;

namespace Commerce.Command.Persistence.Configurations.ImportProduct
{
    /// <summary>
    /// EF Core configuration for ImportProduct entity
    /// </summary>
    public class ProductStockConfig : IEntityTypeConfiguration<Entities.ProductStock>
    {
        public void Configure(EntityTypeBuilder<Entities.ProductStock> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(ImportProductConst.FIELD_PRODUCT_STOCK_ID);

            builder.Property(x => x.ProductDetailId)
                .HasColumnName(ImportProductConst.FIELD_PRODUCT_STOCK_PRODUCT_ID)
                .IsRequired();

            builder.Property(x => x.WareHouseId)
                .HasColumnName(ImportProductConst.FIELD_PRODUCT_STOCK_WAREHOUSE_ID);

            builder.Property(x => x.Quantity)
                .HasColumnName(ImportProductConst.FIELD_PRODUCT_STOCK_QUANTITY);          

            builder.ToTable(ImportProductConst.TABLE_PRODUCT_STOCK);          
        }
    }
}
