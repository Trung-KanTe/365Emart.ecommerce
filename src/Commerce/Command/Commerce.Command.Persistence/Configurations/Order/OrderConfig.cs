using Commerce.Command.Domain.Constants.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Command.Domain.Entities.Order;

namespace Commerce.Command.Persistence.Configurations.Order
{
    /// <summary>
    /// EF Core configuration for Order entity
    /// </summary>
    public class OrderConfig : IEntityTypeConfiguration<Entities.Order>
    {
        public void Configure(EntityTypeBuilder<Entities.Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(OrderConst.FIELD_ORDER_ID);

            builder.Property(x => x.UserId)
                .HasColumnName(OrderConst.FIELD_ORDER_USER_ID)
                .IsRequired();

            builder.Property(x => x.PromotionId)
                .HasColumnName(OrderConst.FIELD_ORDER_PROMOTION_ID);

            builder.Property(x => x.TotalAmount)
                .HasColumnName(OrderConst.FIELD_ORDER_TOTAL_AMOUNT)
                .IsRequired();

            builder.Property(x => x.PaymentMethod)
                .HasColumnName(OrderConst.FIELD_ORDER_PAYMENT_METHOD)
                .HasMaxLength(OrderConst.ORDER_PAYMENT_METHOD_MAX_LENGTH);

            builder.Property(x => x.Address)
                .HasColumnName(OrderConst.FIELD_ORDER_ADDRESS);

            builder.Property(x => x.Status)
                .HasColumnName(OrderConst.FIELD_ORDER_STATUS)
                .HasMaxLength(OrderConst.ORDER_STATUS_MAX_LENGTH);

            builder.Property(x => x.InsertedAt)
                .HasColumnName(OrderConst.FIELD_ORDER_INSERTED_AT);

            builder.Property(x => x.InsertedBy)
                .HasColumnName(OrderConst.FIELD_ORDER_INSERTED_BY);

            builder.Property(x => x.UpdatedAt)
                .HasColumnName(OrderConst.FIELD_ORDER_UPDATED_AT);

            builder.Property(x => x.UpdatedBy)
                .HasColumnName(OrderConst.FIELD_ORDER_UPDATED_BY);

            builder.Property(x => x.IsDeleted)
                .HasColumnName(OrderConst.FIELD_ORDER_IS_DELETED);

            builder.ToTable(OrderConst.TABLE_ORDER);

            builder.HasMany(o => o.OrderItems)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
