namespace Commerce.Query.Contract.Abstractions
{
    /// <summary>
    /// Interface provide result for every use case handled
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// State of this result
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Status code of action
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Message of action if any
        /// </summary>
        public string? Message { get; }

        /// <summary>
        /// Error when result state is false
        /// </summary>
        public IError? Error { get; }
    }
}