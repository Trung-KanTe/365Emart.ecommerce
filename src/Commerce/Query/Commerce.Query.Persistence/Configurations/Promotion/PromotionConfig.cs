using Commerce.Query.Domain.Constants.Promotion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Query.Domain.Entities.Promotion;

namespace Commerce.Query.Persistence.Configurations.Promotion
{
    /// <summary>
    /// EF Core configuration for promotion entity
    /// </summary>
    public class PromotionConfig : IEntityTypeConfiguration<Entities.Promotion>
    {
        public void Configure(EntityTypeBuilder<Entities.Promotion> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(PromotionConst.FIELD_ID);
            builder.Property(x => x.Name).HasColumnName(PromotionConst.FIELD_NAME);
            builder.Property(x => x.Description).HasColumnName(PromotionConst.FIELD_DESCRIPTION);
            builder.Property(x => x.DiscountType).HasColumnName(PromotionConst.FIELD_DISCOUNT_TYPE);
            builder.Property(x => x.DiscountValue).HasColumnName(PromotionConst.FIELD_DISCOUNT_VALUE);
            builder.Property(x => x.StartDate).HasColumnName(PromotionConst.FIELD_START_DATE);
            builder.Property(x => x.EndDate).HasColumnName(PromotionConst.FIELD_END_DATE);
            builder.Property(x => x.InsertedBy).HasColumnName(PromotionConst.FIELD_INSERTED_BY);
            builder.Property(x => x.InsertedAt).HasColumnName(PromotionConst.FIELD_INSERTED_AT);
            builder.Property(x => x.UpdatedAt).HasColumnName(PromotionConst.FIELD_UPDATED_AT);
            builder.Property(x => x.UpdatedBy).HasColumnName(PromotionConst.FIELD_UPDATED_BY);
            builder.Property(x => x.IsDeleted).HasColumnName(PromotionConst.FIELD_DELETE);
            builder.ToTable(PromotionConst.TABLE_PROMOTION);
        }
    }
}
