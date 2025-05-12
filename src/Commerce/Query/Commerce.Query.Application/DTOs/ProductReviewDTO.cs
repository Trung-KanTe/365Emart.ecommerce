using Commerce.Command.Domain.Entities.Product;

namespace Commerce.Query.Application.DTOs
{
    public class ProductReviewDTO
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? UserId { get; set; }
        public int? Rating { get; set; } 
        public string? Comment { get; set; }
        public string? Image { get; set; }
        public DateTime? InsertedAt { get; set; }
        public UserDTO User { get; set; }
        public ProductReviewReply ProductReviewReply { get; set; }

    }
}