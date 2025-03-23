namespace Commerce.Query.Application.DTOs
{
    public class ProductsDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public ICollection<ProductDetailDTO>? ProductDetails { get; set; }
    }
}