using Commerce.Command.Domain.Constants.Product;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities = Commerce.Command.Domain.Entities.Product;

namespace Commerce.Command.Persistence.Configurations.Product
{
    /// <summary>
    /// EF Core configuration for ProductReview entity
    /// </summary>
    public class ProductReviewConfig : IEntityTypeConfiguration<Entities.ProductReview>
    {
        public void Configure(EntityTypeBuilder<Entities.ProductReview> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_ID);

            // Thuộc tính ProductId (Liên kết với Product)
            builder.Property(x => x.ProductId)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_PRODUCT_ID);

            // Thuộc tính UserId (Liên kết với User)
            builder.Property(x => x.UserId)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_USER_ID);

            // Thuộc tính Rating
            builder.Property(x => x.Rating)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_RATING);

            // Thuộc tính Comment
            builder.Property(x => x.Comment)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_COMMENT);

            // Thuộc tính InsertedAt
            builder.Property(x => x.InsertedAt)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_INSERTED_AT);

            // Thuộc tính InsertedBy
            builder.Property(x => x.InsertedBy)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_INSERTED_BY);

            // Thuộc tính UpdatedAt
            builder.Property(x => x.UpdatedAt)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_UPDATED_AT);

            // Thuộc tính UpdatedBy
            builder.Property(x => x.UpdatedBy)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_UPDATED_BY);

            // Thuộc tính IsDeleted
            builder.Property(x => x.IsDeleted)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_IS_DELETED);

            // Thiết lập tên bảng
            builder.ToTable(ProductConst.TABLE_PRODUCT_REVIEW);
        }
    }
}
