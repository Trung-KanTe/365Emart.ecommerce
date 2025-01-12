namespace Commerce.Command.Presentation.DTOs.Promotion
{
    public class UpdatePromotionRequestDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
