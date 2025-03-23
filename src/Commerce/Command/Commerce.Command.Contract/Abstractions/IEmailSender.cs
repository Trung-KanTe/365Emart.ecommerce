namespace Commerce.Command.Contract.Abstractions
{
    public interface IEmailSender
    {
        Task SendOrderConfirmationEmailAsync(string toEmail, Guid orderId);
        Task SendUserForgotPassword(string toEmail, string name, string resetToken);
    }
}