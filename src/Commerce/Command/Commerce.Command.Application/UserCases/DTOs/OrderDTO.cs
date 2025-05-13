using Commerce.Command.Domain.Entities.Order;

namespace Commerce.Command.Application.UserCases.DTOs
{
    public class OrderDTO
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PromotionId { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? DiscountValue { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; }
        public DateTime? InsertedAt { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } 
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}