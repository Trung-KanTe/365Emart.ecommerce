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
        public string? ProductName { get; set; }
    }
}