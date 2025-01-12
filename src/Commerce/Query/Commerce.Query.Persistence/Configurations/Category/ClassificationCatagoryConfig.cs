using Commerce.Query.Domain.Constants.Category;
using Commerce.Query.Domain.Entities.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commerce.Query.Persistence.Configurations.Category
{
    /// <summary>
    /// EF core configuration for classificationCatagory entity
    /// </summary>
    class ClassificationCatagoryConfig : IEntityTypeConfiguration<ClassificationCategory>
    {
        public void Configure(EntityTypeBuilder<ClassificationCategory> builder)
        {
            builder.ToTable(CategoryConst.TABLE_CLASSIFICATION_CATEGORY);
            builder.Property(x => x.CategoryId).HasColumnName(CategoryConst.FIELD_CATEGORY_ID);
            builder.Property(x => x.ClassificationId).HasColumnName(CategoryConst.FIELD_CLASSIFICATION_ID);
            builder.HasKey(x => new
            {
                x.CategoryId,
                x.ClassificationId,
            });
            
            builder.HasOne(cc => cc.Category).WithMany(c => c.ClassificationCategories).HasForeignKey(cc => cc.CategoryId).OnDelete(DeleteBehavior.Cascade); 
        }
    }
}