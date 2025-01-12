namespace Commerce.Command.Presentation.DTOs.Order
{
    public class UpdateOrderCancelRequestDTO
    {
        public Guid? OrderId { get; set; }
        public string? Reason { get; set; }
        public decimal? RefundAmount { get; set; }
        public bool IsRefunded { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
