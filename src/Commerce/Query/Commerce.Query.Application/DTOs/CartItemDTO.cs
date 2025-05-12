namespace Commerce.Query.Application.DTOs
{
    public class CartItemDTO
    {
        public Guid? Id { get; set; }
        public Guid? CartId { get; set; }
        public Guid? ProductDetailId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public decimal? Total { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public Guid? ShopId { get; set; }
        public string? ShopName { get; set; }
        public ProductDetailDTO? ProductDetails { get; set; }
    }
}