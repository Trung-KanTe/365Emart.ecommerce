using Commerce.Query.Domain.Constants.Cart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Query.Domain.Entities.Cart;

namespace Commerce.Query.Persistence.Configurations.Cart
{
    /// <summary>
    /// EF Core configuration for CartItem entity
    /// </summary>
    public class CartItemConfig : IEntityTypeConfiguration<Entities.CartItem>
    {
        public void Configure(EntityTypeBuilder<Entities.CartItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(CartConst.FIELD_CART_ITEM_ID);

            builder.Property(x => x.CartId)
                .HasColumnName(CartConst.FIELD_CART_ITEM_CART_ID)
                .IsRequired();

            builder.Property(x => x.ProductId)
                .HasColumnName(CartConst.FIELD_CART_ITEM_PRODUCT_ID)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName(CartConst.FIELD_CART_ITEM_PRICE)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasColumnName(CartConst.FIELD_CART_ITEM_QUANTITY)
                .IsRequired();

            builder.Property(x => x.Total)
                .HasColumnName(CartConst.FIELD_CART_ITEM_TOTAL)
                .IsRequired();

            builder.ToTable(CartConst.TABLE_CART_ITEMS);

            builder.HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
