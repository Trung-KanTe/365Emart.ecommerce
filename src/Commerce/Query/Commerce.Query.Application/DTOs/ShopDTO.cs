namespace Commerce.Query.Application.DTOs
{
    public class ShopDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Style { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public Guid? WardId { get; set; }
        public Guid? UserId { get; set; }
        public int? Views { get; set; } 
        public DateTime? InsertedAt { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } 
    }
}