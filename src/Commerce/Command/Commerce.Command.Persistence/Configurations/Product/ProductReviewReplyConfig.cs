using Commerce.Command.Domain.Constants.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Command.Domain.Entities.Product;

namespace Commerce.Command.Persistence.Configurations.Product
{
    /// <summary>
    /// EF Core configuration for ProductReviewReply entity
    /// </summary>
    public class ProductReviewReplyConfig : IEntityTypeConfiguration<Entities.ProductReviewReply>
    {
        public void Configure(EntityTypeBuilder<Entities.ProductReviewReply> builder)
        {
            // Khóa chính
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_REPLY_ID);

            // Thuộc tính ReviewId (liên kết với ProductReview)
            builder.Property(x => x.ReviewId)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_REPLY_REVIEW_ID);

            // Thuộc tính ShopId (liên kết với Shop)
            builder.Property(x => x.ShopId)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_REPLY_SHOP_ID);

            // Thuộc tính Reply (nội dung phản hồi)
            builder.Property(x => x.Reply)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_REPLY_REPLY);

            // Timestamps & Audit
            builder.Property(x => x.InsertedAt)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_REPLY_INSERTED_AT);

            builder.Property(x => x.InsertedBy)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_REPLY_INSERTED_BY);

            builder.Property(x => x.UpdatedAt)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_REPLY_UPDATED_AT);

            builder.Property(x => x.UpdatedBy)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_REPLY_UPDATED_BY);

            builder.Property(x => x.IsDeleted)
                .HasColumnName(ProductConst.FIELD_PRODUCT_REVIEW_REPLY_IS_DELETED);

            // Tên bảng
            builder.ToTable(ProductConst.TABLE_PRODUCT_REVIEW_REPLY);
        }
    }
}
