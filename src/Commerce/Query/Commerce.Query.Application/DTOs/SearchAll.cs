namespace Commerce.Query.Application.DTOs
{
    public class SearchAll
    {
        public List<ProductDTO>? Products { get; set; }
        public BrandDTO? brand { get; set; }
        public ShopDTO? shop { get; set; }
    }
}