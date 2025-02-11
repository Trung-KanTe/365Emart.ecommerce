using Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.DTOs
{
    public class ProductDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Views { get; set; }
        public decimal? Price { get; set; }
        public CatDTO? Category { get; set; }
        public BrandDTO? Brand { get; set; }
        public ShopDTO? Shop { get; set; }
        public string? Image { get; set; }
        public DateTime? InsertedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public ICollection<ProductDetail>? ProductDetails { get; set; }
    }
}