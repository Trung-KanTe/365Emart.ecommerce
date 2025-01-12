using Commerce.Query.Domain.Entities.Payment;

namespace Query.DTOs
{
    public class PaymentDTO
    {
        public Guid? Id { get; set; }
        public Guid? OrderId { get; set; }
        public decimal? Amount { get; set; }
        public string? TransactionId { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public List<PaymentDetails>? PaymentDetails { get; set; }
    }
}