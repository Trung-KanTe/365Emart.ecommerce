namespace Commerce.Query.Application.DTOs
{
    public class OrderItemDTO
    {
        public Guid? Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? ProductDetailId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }
        public Guid? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImage { get; set; }
        public ProductDetailDTO ProductDetails { get; set; }
    }
}