using Commerce.Command.Domain.Entities.Cart;

namespace Commerce.Command.Presentation.DTOs.Cart
{
    public class UpdateCartRequestDTO
    {
        public Guid? UserId { get; set; }
        public int? TotalQuantity { get; set; } = 0;
        public bool? IsDeleted { get; set; } = false;
        public ICollection<CartItem>? CartItems { get; set; }
    }
}
