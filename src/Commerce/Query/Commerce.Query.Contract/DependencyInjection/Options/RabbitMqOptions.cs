namespace Commerce.Query.Contract.DependencyInjection.Options
{
    public class RabbitMqOptions
    {
        public string Host { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}