using Commerce.Query.Domain.Entities.Category;

namespace Commerce.Query.Application.DTOs
{
    public class CategoryDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Style { get; set; }
        public Guid? UserId { get; set; }
        public int? Views { get; set; }
        public DateTime? InsertedAt { get; set; }
        public bool? IsDeleted { get; set; } 
        public ICollection<Classification>? Classifications { get; set; }
    }
}