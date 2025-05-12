namespace Commerce.Query.Application.DTOs
{
    public class ShopCartDTO
    {
        public Guid? ShopId { get; set; }
        public string? ShopName { get; set; }
        public List<CartItemDTO> CartItems { get; set; } = new();
    }
}