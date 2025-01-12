using Commerce.Query.Domain.Constants.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Query.Domain.Entities.Category;

namespace Commerce.Query.Persistence.Configurations.Category
{
    /// <summary>
    /// EF core configuration for category entity
    /// </summary>
    public class CategoryConfig : IEntityTypeConfiguration<Entities.Category>
    {
        public void Configure(EntityTypeBuilder<Entities.Category> builder)
        {
            builder.ToTable(CategoryConst.TABLE_CATEGORY);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(CategoryConst.FIELD_CATEGORY_ID);
            builder.Property(x => x.Name).HasColumnName(CategoryConst.FIELD_CATEGORY_NAME).HasMaxLength(CategoryConst.NAME_MAX_LENGTH);
            builder.Property(x => x.Image).HasColumnName(CategoryConst.FIELD_CATEGORY_IMAGE).HasMaxLength(CategoryConst.IMAGE_MAX_LENGTH);           
            builder.Property(x => x.Style).HasColumnName(CategoryConst.FIELD_CATEGORY_STYLE).HasMaxLength(CategoryConst.ICON_MAX_LENGTH);
            builder.Property(x => x.UserId).HasColumnName(CategoryConst.FIELD_CATEGORY_USER_ID);
            builder.Property(x => x.Views).HasColumnName(CategoryConst.FIELD_CATEGORY_VIEWS);
            builder.Property(x => x.InsertedBy).HasColumnName(CategoryConst.FIELD_CATEGORY_INSERTED_BY);
            builder.Property(x => x.InsertedAt).HasColumnName(CategoryConst.FIELD_CATEGORY_INSERTED_AT);
            builder.Property(x => x.UpdatedAt).HasColumnName(CategoryConst.FIELD_CATEGORY_UPDATED_AT);
            builder.Property(x => x.UpdatedBy).HasColumnName(CategoryConst.FIELD_CATEGORY_UPDATED_BY);
            builder.Property(x => x.IsDeleted).HasColumnName(CategoryConst.FIELD_CATEGORY_IS_DELETED);
            builder.HasMany(c => c.ClassificationCategories).WithOne(cc => cc.Category).HasForeignKey(cc => cc.CategoryId).OnDelete(DeleteBehavior.Cascade);           
        }
    }
}