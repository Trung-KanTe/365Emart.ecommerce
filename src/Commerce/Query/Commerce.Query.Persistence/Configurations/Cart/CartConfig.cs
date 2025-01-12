using Commerce.Query.Domain.Constants.Cart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Query.Domain.Entities.Cart;

namespace Commerce.Query.Persistence.Configurations.Cart
{
    /// <summary>
    /// EF Core configuration for Cart entity
    /// </summary>
    public class CartConfig : IEntityTypeConfiguration<Entities.Cart>
    {
        public void Configure(EntityTypeBuilder<Entities.Cart> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(CartConst.FIELD_CART_ID);

            builder.Property(x => x.UserId)
                .HasColumnName(CartConst.FIELD_CART_USER_ID)
                .IsRequired();

            builder.Property(x => x.TotalQuantity)
                .HasColumnName(CartConst.FIELD_CART_TOTAL_QUANTITY)
                .IsRequired();

            builder.Property(x => x.InsertedAt)
                .HasColumnName(CartConst.FIELD_CART_INSERTED_AT);

            builder.Property(x => x.InsertedBy)
                .HasColumnName(CartConst.FIELD_CART_INSERTED_BY);

            builder.Property(x => x.UpdatedAt)
                .HasColumnName(CartConst.FIELD_CART_UPDATED_AT);

            builder.Property(x => x.UpdatedBy)
                .HasColumnName(CartConst.FIELD_CART_UPDATED_BY);

            builder.Property(x => x.IsDeleted)
                .HasColumnName(CartConst.FIELD_CART_IS_DELETED);

            builder.ToTable(CartConst.TABLE_SHOPPING_CART);

            builder.HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
