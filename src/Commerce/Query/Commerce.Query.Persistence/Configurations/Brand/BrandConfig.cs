using Commerce.Query.Domain.Constants.Brand;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Query.Domain.Entities.Brand;

namespace Commerce.Query.Persistence.Configurations.Brand
{
    /// <summary>
    /// EF core configuration for brand entity
    /// </summary>
    public class BrandConfig : IEntityTypeConfiguration<Entities.Brand>
    {
        public void Configure(EntityTypeBuilder<Entities.Brand> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(BrandConst.FIELD_ID);
            builder.Property(x => x.Name).HasColumnName(BrandConst.FIELD_NAME);
            builder.Property(x => x.Icon).HasColumnName(BrandConst.FIELD_ICON);
            builder.Property(x => x.Style).HasColumnName(BrandConst.FIELD_STYLE);
            builder.Property(x => x.UserId).HasColumnName(BrandConst.FIELD_USER_ID);
            builder.Property(x => x.Views).HasColumnName(BrandConst.FIELD_VIEWS);
            builder.Property(x => x.InsertedBy).HasColumnName(BrandConst.FIELD_INSERTED_BY);
            builder.Property(x => x.InsertedAt).HasColumnName(BrandConst.FIELD_INSERTED_AT);
            builder.Property(x => x.UpdatedAt).HasColumnName(BrandConst.FIELD_UPDATED_AT);
            builder.Property(x => x.UpdatedBy).HasColumnName(BrandConst.FIELD_UPDATED_BY);
            builder.Property(x => x.IsDeleted).HasColumnName(BrandConst.FIELD_DELETE);
            builder.ToTable(BrandConst.TABLE_BRAND);
        }
    }
}