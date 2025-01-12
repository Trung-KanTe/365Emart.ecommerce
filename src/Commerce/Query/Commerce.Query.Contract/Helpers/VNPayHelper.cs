using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Commerce.Query.Contract.Helpers
{
    public class VNPayHelper
    {
        private readonly IConfiguration configuration;

        public VNPayHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CreatePaymentUrl(VNPayRequest request)
        {
            string baseUrl = configuration["VNPay:vnp_Url"]!;
            string vnp_TmnCode = configuration["VNPay:vnp_TmnCode"]!;
            string vnp_HashSecret = configuration["VNPay:vnp_HashSecret"]!;
            string vnp_ReturnUrl = configuration["VNPay:vnp_ReturnUrl"]!;

            var vnPayData = new SortedList<string, string>
        {
            {"vnp_Version", "2.1.0"},
            {"vnp_Command", "pay"},
            {"vnp_TmnCode", vnp_TmnCode},
            {"vnp_Amount", (Convert.ToInt32(request.Amount) * 100).ToString()},
            {"vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")},
            {"vnp_CurrCode", "VND"},
            {"vnp_IpAddr", request.IpAddress},
            {"vnp_Locale", "vn"},
            {"vnp_OrderInfo", request.OrderInfo},
            {"vnp_OrderType", "billpayment"},
            {"vnp_ReturnUrl", vnp_ReturnUrl},
            {"vnp_TxnRef", request.OrderId}
        };

            string query = string.Join("&", vnPayData.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
            string hash = CreateHash(query, vnp_HashSecret);

            return $"{baseUrl}?{query}&vnp_SecureHash={hash}";
        }

        private string CreateHash(string data, string secretKey)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey));
            byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashValue).Replace("-", "").ToUpper();
        }
    }
}
