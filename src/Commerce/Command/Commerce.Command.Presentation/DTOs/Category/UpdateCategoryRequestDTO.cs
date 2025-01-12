namespace Commerce.Command.Presentation.DTOs.Category
{
    public class UpdateCategoryRequestDTO
    {
        public string? Name { get; set; }
        public string? Image { get; set; } = null;
        public string? Style { get; set; } = null;
        public Guid? UserId { get; set; }
        public int? Views { get; set; } = 0;
        public bool? IsDeleted { get; set; } = false;
        public List<Guid>? ClassificationIds { get; set; }
    }
}
