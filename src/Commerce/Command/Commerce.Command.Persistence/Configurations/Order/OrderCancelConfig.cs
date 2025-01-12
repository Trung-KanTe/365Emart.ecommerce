using Commerce.Command.Domain.Constants.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Command.Domain.Entities.Order;

namespace Commerce.Command.Persistence.Configurations.Order
{
    /// <summary>
    /// EF Core configuration for OrderCancel entity
    /// </summary>
    public class OrderCancelConfig : IEntityTypeConfiguration<Entities.OrderCancel>
    {
        public void Configure(EntityTypeBuilder<Entities.OrderCancel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(OrderConst.FIELD_ORDER_CANCEL_ID);

            builder.Property(x => x.OrderId)
                   .HasColumnName(OrderConst.FIELD_ORDER_CANCEL_ORDER_ID)
                   .IsRequired();

            builder.Property(x => x.Reason)
                   .HasColumnName(OrderConst.FIELD_ORDER_CANCEL_REASON)
                   .HasMaxLength(OrderConst.ORDER_CANCEL_REASON_MAX_LENGTH)
                   .IsRequired();

            builder.Property(x => x.RefundAmount)
                   .HasColumnName(OrderConst.FIELD_ORDER_CANCEL_REFUND_AMOUNT)
                   .IsRequired();

            builder.Property(x => x.IsRefunded)
                   .HasColumnName(OrderConst.FIELD_ORDER_CANCEL_IS_REFUNDED)
                   .IsRequired();

            builder.Property(x => x.InsertedAt)
                   .HasColumnName(OrderConst.FIELD_ORDER_CANCEL_INSERTED_AT)
                   .IsRequired();

            builder.Property(x => x.InsertedBy)
                   .HasColumnName(OrderConst.FIELD_ORDER_CANCEL_INSERTED_BY);

            builder.Property(x => x.UpdatedAt)
                   .HasColumnName(OrderConst.FIELD_ORDER_CANCEL_UPDATED_AT);

            builder.Property(x => x.UpdatedBy)
                   .HasColumnName(OrderConst.FIELD_ORDER_CANCEL_UPDATED_BY);

            builder.Property(x => x.IsDeleted)
                   .HasColumnName(OrderConst.FIELD_ORDER_CANCEL_IS_DELETED)
                   .IsRequired();

            builder.ToTable(OrderConst.TABLE_ORDER_CANCEL);

        }
    }
}
