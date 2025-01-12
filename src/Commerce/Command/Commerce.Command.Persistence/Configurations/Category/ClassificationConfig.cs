using Commerce.Command.Domain.Constants.Category;
using Commerce.Command.Domain.Entities.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commerce.Command.Persistence.Configurations.Category
{
    /// <summary>
    /// EF core configuration for classification entity
    /// </summary>
    public class ClassificationConfig : IEntityTypeConfiguration<Classification>
    {
        public void Configure(EntityTypeBuilder<Classification> builder)
        {
            builder.ToTable(CategoryConst.TABLE_CLASSIFICATION);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(CategoryConst.FIELD_CLASSIFICATION_ID);
            builder.Property(x => x.Name).HasColumnName(CategoryConst.FIELD_CLASSIFICATION_NAME);
            builder.Property(x => x.Icon).HasColumnName(CategoryConst.FIELD_CLASSIFICATION_ICON);
            builder.Property(x => x.Style).HasColumnName(CategoryConst.FIELD_CLASSIFICATION_STYLE);
            builder.Property(x => x.Views).HasColumnName(CategoryConst.FIELD_CLASSIFICATION_VIEWS);
            builder.Property(x => x.InsertedBy).HasColumnName(CategoryConst.FIELD_CLASSIFICATION_INSERTED_BY);
            builder.Property(x => x.InsertedAt).HasColumnName(CategoryConst.FIELD_CLASSIFICATION_INSERTED_AT);
            builder.Property(x => x.UpdatedAt).HasColumnName(CategoryConst.FIELD_CLASSIFICATION_UPDATED_AT);
            builder.Property(x => x.UpdatedBy).HasColumnName(CategoryConst.FIELD_CLASSIFICATION_UPDATED_BY);
            builder.Property(x => x.IsDeleted).HasColumnName(CategoryConst.FIELD_CLASSIFICATION_IS_DELETED);
        }
    }
}