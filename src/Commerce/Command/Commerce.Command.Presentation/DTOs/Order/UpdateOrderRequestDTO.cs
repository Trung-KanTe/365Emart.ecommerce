using Commerce.Command.Domain.Entities.Order;

namespace Commerce.Command.Presentation.DTOs.Order
{
    public class UpdateOrderRequestDTO
    {
        public Guid? UserId { get; set; }
        public Guid? PromotionId { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
