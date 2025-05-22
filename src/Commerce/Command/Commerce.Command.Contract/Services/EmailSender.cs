using Commerce.Command.Contract.Abstractions;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Commerce.Command.Contract.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("EmailSettings");
        }

        public async Task SendOrderConfirmationEmailAsync(string toEmail, Guid orderId)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("365-Emart", _configuration["SenderEmail"]));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = "Xác nhận đơn hàng #" + orderId;

            email.Body = new TextPart("html")
            {
                Text = $"<h2>365-Emart xin chân thành cảm ơn bạn đã đặt hàng!</h2><p>Đơn hàng <b>#{orderId}</b> của bạn đang được xử lý.</p>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_configuration["SmtpServer"], int.Parse(_configuration["SmtpPort"]!), false);
            await smtp.AuthenticateAsync(_configuration["SenderEmail"], _configuration["SenderPassword"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendUserForgotPassword(string toEmail, string name, string resetToken)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("365-Emart", _configuration["SenderEmail"]));
            email.To.Add(new MailboxAddress(name, toEmail));
            email.Subject = "🔑 Đặt lại mật khẩu - 365-Emart";

            string resetLink = $"http://localhost:4200/password?token={resetToken}";

            email.Body = new TextPart("html")
            {
                Text = $@"
                    <h2>Xin chào {name},</h2>
                    <p>Bạn đã yêu cầu đặt lại mật khẩu cho tài khoản của mình.</p>
                    <p>Vui lòng nhấp vào liên kết dưới đây để đặt lại mật khẩu mới:</p>
                    <p><a href='{resetLink}' style='font-size:16px; color:#fff; background:#007bff; padding:10px 20px; text-decoration:none; border-radius:5px;'>🔗 Reset password</a></p>
                    <p>Liên kết này có hiệu lực trong <b>15 phút</b>.</p>
                    <p>Nếu bạn không yêu cầu đặt lại mật khẩu, hãy bỏ qua email này.</p>
                    <p>Trân trọng,<br> <b>365-Emart team</b></p>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_configuration["SmtpServer"], int.Parse(_configuration["SmtpPort"]!), false);
            await smtp.AuthenticateAsync(_configuration["SenderEmail"], _configuration["SenderPassword"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("365-Emart", _configuration["SenderEmail"]));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = htmlBody };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_configuration["SmtpServer"], int.Parse(_configuration["SmtpPort"]!), false);
            await smtp.AuthenticateAsync(_configuration["SenderEmail"], _configuration["SenderPassword"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}