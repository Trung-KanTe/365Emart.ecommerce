using Commerce.Query.Domain.Constants.Ward;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Query.Domain.Entities.Ward;

namespace Commerce.Query.Persistence.Configurations.Ward
{
    /// <summary>
    /// EF Core configuration for Ward entity
    /// </summary>
    public class ProvinceConfig : IEntityTypeConfiguration<Entities.Province>
    {
        public void Configure(EntityTypeBuilder<Entities.Province> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(WardConst.FIELD_PROVINCE_ID);
            builder.Property(x => x.Name).HasColumnName(WardConst.FIELD_PROVINCE_NAME);
            builder.Property(x => x.FullName).HasColumnName(WardConst.FIELD_PROVINCE_FULL_NAME);
            builder.Property(x => x.LocalId).HasColumnName(WardConst.FIELD_PROVINCE_KEY);
            builder.ToTable(WardConst.TABLE_PROVINCE);
        }
    }
}
