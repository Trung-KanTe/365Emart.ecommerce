namespace Commerce.Command.Presentation.DTOs.Payment
{
    public class CreatePaymentRequestDto
    {
        public double Money { get; set; }
        public string Description { get; set; }
        public List<Guid> OrderIds { get; set; }
    }
}