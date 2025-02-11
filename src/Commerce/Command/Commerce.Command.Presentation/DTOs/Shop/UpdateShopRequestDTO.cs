namespace Commerce.Command.Presentation.DTOs.Shop
{
    public class UpdateShopRequestDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Style { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public int Views { get; set; }
        public Guid? WardId { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
