using Commerce.Command.Domain.Entities.Product;

namespace Commerce.Command.Presentation.DTOs.Product
{
    public class UpdateProductRequestDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Views { get; set; } = 0;
        public decimal? Price { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ShopId { get; set; }
        public string? Image { get; set; }
        public bool? IsDeleted { get; set; } = true;
        public ICollection<ProductDetail>? ProductDetails { get; set; }
    }
}
