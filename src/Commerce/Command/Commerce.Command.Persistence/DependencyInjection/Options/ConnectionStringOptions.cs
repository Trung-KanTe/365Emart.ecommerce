namespace Commerce.Command.Persistence.DependencyInjection.Options
{
    /// <summary>
    /// Options for connection string
    /// </summary>
    public class ConnectionStringOptions
    {
        public const string ConnectionStrings = "ConnectionStrings";
        public string SqlServer { get; set; } = string.Empty;
    }
}