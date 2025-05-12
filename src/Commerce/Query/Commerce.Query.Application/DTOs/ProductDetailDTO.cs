namespace Commerce.Query.Application.DTOs
{
    public class ProductDetailDTO
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public int? StockQuantity { get; set; }
    }
}