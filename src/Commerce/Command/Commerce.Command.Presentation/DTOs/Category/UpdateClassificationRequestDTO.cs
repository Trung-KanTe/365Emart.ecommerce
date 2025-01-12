namespace Commerce.Command.Presentation.DTOs.Category
{
    public class UpdateClassificationRequestDTO
    {
        public string? Name { get; set; }
        public string? Icon { get; set; } = null;
        public string? Style { get; set; } = null;
        public int? Views { get; set; } = 0;
        public bool? IsDeleted { get; set; } = false;
    }
}
