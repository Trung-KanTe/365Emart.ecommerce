namespace Commerce.Query.Contract.Exceptions
{
    /// <summary>
    /// Provide validation exceptions of domain entity
    /// </summary>
    public class DomainValidationException : Exception
    {
        /// <summary>
        /// Provide details of exception
        /// </summary>
        public List<string> Details { get; } = new();

        public DomainValidationException(params string[]? exceptionDetail) : base("Domain validation occurred")
        {
            if (exceptionDetail is not null)
                Details.AddRange(exceptionDetail);
        }
    }
}