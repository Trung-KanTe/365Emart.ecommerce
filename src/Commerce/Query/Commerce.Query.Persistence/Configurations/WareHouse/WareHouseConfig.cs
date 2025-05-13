using Commerce.Query.Domain.Constants.WareHouse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities = Commerce.Query.Domain.Entities.WareHouse;

namespace Commerce.Query.Persistence.Configurations.WareHouse
{
    /// <summary>
    /// EF Core configuration for WareHouse entity
    /// </summary>
    public class WareHouseConfig : IEntityTypeConfiguration<Entities.WareHouse>
    {
        public void Configure(EntityTypeBuilder<Entities.WareHouse> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(WareHouseConst.FIELD_WAREHOUSE_ID);

            builder.Property(x => x.Name)
                .HasColumnName(WareHouseConst.FIELD_WAREHOUSE_NAME)
                .HasMaxLength(WareHouseConst.WAREHOUSE_NAME_MAX_LENGTH);

            builder.Property(x => x.Address)
                .HasColumnName(WareHouseConst.FIELD_WAREHOUSE_ADDRESS)
                .HasMaxLength(WareHouseConst.WAREHOUSE_ADDRESS_MAX_LENGTH);

            builder.Property(x => x.WardId)
                .HasColumnName(WareHouseConst.FIELD_WAREHOUSE_WARD_ID);

            builder.Property(x => x.ShopId)
                .HasColumnName(WareHouseConst.FIELD_WAREHOUSE_SHOP_ID);

            builder.Property(x => x.InsertedAt)
                .HasColumnName(WareHouseConst.FIELD_WAREHOUSE_INSERTED_AT);

            builder.Property(x => x.InsertedBy)
                .HasColumnName(WareHouseConst.FIELD_WAREHOUSE_INSERTED_BY);

            builder.Property(x => x.UpdatedAt)
                .HasColumnName(WareHouseConst.FIELD_WAREHOUSE_UPDATED_AT);

            builder.Property(x => x.UpdatedBy)
                .HasColumnName(WareHouseConst.FIELD_WAREHOUSE_UPDATED_BY);

            builder.Property(x => x.IsDeleted)
                .HasColumnName(WareHouseConst.FIELD_WAREHOUSE_IS_DELETED);

            builder.ToTable(WareHouseConst.TABLE_WAREHOUSE);
        }
    }
}
