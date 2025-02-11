using VNPAY.NET;

namespace Commerce.Command.Contract.Helpers
{
    public class VnpayPayment
    {
        private string _tmnCode;
        private string _hashSecret;
        private string _baseUrl;
        private string _callbackUrl;

        private readonly IVnpay _vnpay;

        public VnpayPayment(IVnpay vnpay)
        {
            _vnpay = vnpay;
            _vnpay.Initialize(_tmnCode, _hashSecret, _baseUrl, _callbackUrl);
        }
    }
}