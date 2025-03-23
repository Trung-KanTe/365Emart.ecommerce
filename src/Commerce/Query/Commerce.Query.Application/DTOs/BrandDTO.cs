namespace Commerce.Query.Application.DTOs
{
    public class BrandDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public string? Style { get; set; }
        public Guid? UserId { get; set; }
        public int? Views { get; set; }
        public DateTime? InsertedAt { get; set; }
        public Guid? InsertedBy { get; set; }
        public bool? IsDeleted { get; set; } 
        public List<ProductDTO> Products { get; set; }
    }
}