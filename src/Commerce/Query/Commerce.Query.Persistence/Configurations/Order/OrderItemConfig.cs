using Commerce.Query.Domain.Constants.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Query.Domain.Entities.Order;

namespace Commerce.Query.Persistence.Configurations.Order
{
    /// <summary>
    /// EF Core configuration for OrderItem entity
    /// </summary>
    public class OrderItemConfig : IEntityTypeConfiguration<Entities.OrderItem>
    {
        public void Configure(EntityTypeBuilder<Entities.OrderItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(OrderConst.FIELD_ORDER_ITEM_ID);

            builder.Property(x => x.OrderId)
                .HasColumnName(OrderConst.FIELD_ORDER_ITEM_ORDER_ID)
                .IsRequired();

            builder.Property(x => x.ProductId)
                .HasColumnName(OrderConst.FIELD_ORDER_ITEM_PRODUCT_ID)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasColumnName(OrderConst.FIELD_ORDER_ITEM_QUANTITY)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName(OrderConst.FIELD_ORDER_ITEM_PRICE)
                .IsRequired();

            builder.Property(x => x.Total)
                .HasColumnName(OrderConst.FIELD_ORDER_ITEM_TOTAL);

            builder.ToTable(OrderConst.TABLE_ORDER_ITEM);

            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
