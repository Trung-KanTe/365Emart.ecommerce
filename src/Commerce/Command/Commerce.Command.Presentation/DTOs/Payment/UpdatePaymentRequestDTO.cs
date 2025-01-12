using Commerce.Command.Domain.Entities.Payment;

namespace Commerce.Command.Presentation.DTOs.Payment
{
    public class UpdatePaymentRequestDTO
    {
        public Guid? OrderId { get; set; }
        public decimal? Amount { get; set; }
        public string? TransactionId { get; set; }
        public string? ReturnUrl { get; set; }
        public string? OrderInfo { get; set; }
        public string? IpAddress { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public string? ResponseCode { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public ICollection<PaymentDetails>? PaymentDetails { get; set; }
    }
}
