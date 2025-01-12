namespace Commerce.Command.Presentation.DTOs.Brand
{
    /// <summary>
    /// DTO for request incoming from end user
    /// </summary>
    public class UpdateBrandRequestDTO
    {
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public string? Style { get; set; }
        public Guid? UserId { get; set; }
        public int? Views { get; set; }
        public bool? IsDeleted { get; set; }
    }
}