namespace Commerce.Query.Contract.Helpers
{
    public class VNPayRequest
    {
        public string Amount { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public string ReturnUrl { get; set; }
        public string IpAddress { get; set; }
    }
}