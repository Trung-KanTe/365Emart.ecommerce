namespace Commerce.Command.Presentation.DTOs.Product
{
    public class UpdateProductReviewRequestDTO
    {
        public Guid? ProductId { get; set; }
        public Guid? UserId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
