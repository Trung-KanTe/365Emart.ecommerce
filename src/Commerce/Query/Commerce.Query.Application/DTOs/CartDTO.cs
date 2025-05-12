namespace Commerce.Query.Application.DTOs
{
    public class CartDTO
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public int? TotalQuantity { get; set; }
        public List<ShopCartDTO> ShopCarts { get; set; } = new();
    }
}