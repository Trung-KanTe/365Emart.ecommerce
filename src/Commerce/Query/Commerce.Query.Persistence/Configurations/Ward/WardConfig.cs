using Commerce.Query.Domain.Constants.Ward;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Query.Domain.Entities.Ward;

namespace Commerce.Query.Persistence.Configurations.Ward
{
    /// <summary>
    /// EF Core configuration for Ward entity
    /// </summary>
    public class WardConfig : IEntityTypeConfiguration<Entities.Ward>
    {
        public void Configure(EntityTypeBuilder<Entities.Ward> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(WardConst.FIELD_USER_ID);
            builder.Property(x => x.Name).HasColumnName(WardConst.FIELD_USER_NAME);
            builder.Property(x => x.FullName).HasColumnName(WardConst.FIELD_USER_FULL_NAME);
            builder.Property(x => x.DistrictId).HasColumnName(WardConst.FIELD_USER_DISTRICT);
            builder.ToTable(WardConst.TABLE_USER);         
        }
    }
}
