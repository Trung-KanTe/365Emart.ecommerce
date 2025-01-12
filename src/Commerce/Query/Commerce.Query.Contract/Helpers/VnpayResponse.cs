namespace Commerce.Query.Contract.Helpers
{
    public class VnpayResponse
    {
        public string Vnp_ResponseCode { get; set; }
        public string Vnp_TxnRef { get; set; }
        public decimal Vnp_Amount { get; set; }
        public string Vnp_TransactionNo { get; set; }
        public string Vnp_BankCode { get; set; }
        public string Vnp_TransactionStatus { get; set; }
        public string Vnp_CardNumber { get; set; }
        public string Vnp_ExtraData { get; set; }
        public string Vnp_Message { get; set; }
    }
}
