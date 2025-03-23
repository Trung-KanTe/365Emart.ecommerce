using Asp.Versioning;
using Commerce.Command.Domain.Abstractions.Repositories.Order;
using Commerce.Command.Domain.Abstractions.Repositories.Payment;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;
using Entities = Commerce.Command.Domain.Entities.Payment;

namespace Commerce.Command.Presentation.Service.Payment
{
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.PAYMENT_ROUTE)]
    public class VnpayPayment : ApiController
    {
        private readonly IVnpay _vnpay;
        private readonly IConfiguration _configuration;
        private readonly IPaymentRepository paymentRepository;
        private readonly IOrderRepository orderRepository;

        public VnpayPayment(IVnpay vnPayservice, IConfiguration configuration, IPaymentRepository paymentRepository)
        {
            _vnpay = vnPayservice;
            _configuration = configuration;

            _vnpay.Initialize(_configuration["Vnpay:TmnCode"], _configuration["Vnpay:HashSecret"], _configuration["Vnpay:BaseUrl"], _configuration["Vnpay:CallbackUrl"]);
            this.paymentRepository = paymentRepository;
        }

        /// <summary>
        /// Tạo url thanh toán
        /// </summary>
        /// <param name="money">Số tiền phải thanh toán</param>
        /// <param name="description">Mô tả giao dịch</param>
        /// <returns></returns>
        [HttpGet("CreatePaymentUrl")]
        public async Task<ActionResult<string>> CreatePaymentUrl(double money, string description, Guid orderId)
        {
            try
            {
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext); // Lấy địa chỉ IP của thiết bị thực hiện giao dịch

                var request = new PaymentRequest
                {
                    PaymentId = DateTime.Now.Ticks,
                    Money = money,
                    Description = description,
                    IpAddress = ipAddress,
                    BankCode = BankCode.ANY, // Tùy chọn. Mặc định là tất cả phương thức giao dịch
                    CreatedDate = DateTime.Now, // Tùy chọn. Mặc định là thời điểm hiện tại
                    Currency = Currency.VND, // Tùy chọn. Mặc định là VND (Việt Nam đồng)
                    Language = DisplayLanguage.Vietnamese // Tùy chọn. Mặc định là tiếng Việt
                };

                var payment = new Entities.Payment
                {
                    OrderId = orderId,
                    Amount = (decimal?)money,
                    IpAddress = ipAddress,
                    PaymentMethod = "ATM",
                    PaymentStatus = "Success",
                    OrderInfo = description,
                    BankCode = request.PaymentId,
                    ReturnUrl = "https://localhost:7149/api/v1/payment/PaymentCallback",
                    BankName = "NCB",
                    CardNumber = "9704198526191432198",
                    TransactionId = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0),
                    ResponseCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
            };

                paymentRepository.Create(payment);
                await paymentRepository.SaveChangesAsync(); // Thêm await

                var paymentUrl = _vnpay.GetPaymentUrl(request);

                return Created(paymentUrl, paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Thực hiện hành động sau khi thanh toán. URL này cần được khai báo với VNPAY để API này hoạt đồng (ví dụ: http://localhost:1234/api/Vnpay/IpnAction)
        /// </summary>
        /// <returns></returns>
        [HttpGet("IpnAction")]
        public IActionResult IpnAction()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpay.GetPaymentResult(Request.Query);
                    if (paymentResult.IsSuccess)
                    {
                        var paymentIdString = Request.Query["vnp_TxnRef"].ToString();
                        if (!long.TryParse(paymentIdString, out long paymentId))
                        {
                            return BadRequest("PaymentId không hợp lệ.");
                        }

                        var payment = paymentRepository.GetByBankCodeAsync(paymentId).Result;
                        if (payment == null)
                        {
                            return NotFound($"Không tìm thấy thông tin thanh toán với PaymentId: {paymentResult.PaymentId}");
                        }
                        payment.TransactionId = paymentResult.TransactionId;
                        payment.BankName = "NCB";
                        payment.CardNumber = "9704198526191432198";
                        payment.ResponseCode = paymentResult.Description;
                        paymentRepository.Update(payment);
                        paymentRepository.SaveChangesAsync();
                        return Ok();
                    }

                    // Thực hiện hành động nếu thanh toán thất bại tại đây. Ví dụ: Hủy đơn hàng.
                    return BadRequest("Thanh toán thất bại");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }

        /// <summary>
        /// Trả kết quả thanh toán về cho người dùng
        /// </summary>
        /// <returns></returns>
        [HttpGet("PaymentCallback")]
        public ActionResult<PaymentResult> Callback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpay.GetPaymentResult(Request.Query);

                    if (paymentResult.IsSuccess)
                    {                        
                        return Redirect("http://localhost:4200/paymentSuccess");
                    }

                    return BadRequest(paymentResult);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }
    }
}