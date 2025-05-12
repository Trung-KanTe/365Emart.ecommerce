using Commerce.Query.Domain.Entities.Order;

namespace Commerce.Query.Application.DTOs
{
    public class OrderDTO
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PromotionId { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public Guid? ShopId { get; set; }
        public string? ShopName { get; set; }
        public string? Status { get; set; }
        public DateTime? InsertedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public List<OrderItemDTO>? OrderItems { get; set; }
    }
}
