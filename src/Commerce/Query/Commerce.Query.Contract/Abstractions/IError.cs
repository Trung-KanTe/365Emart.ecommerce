using System.Text.Json.Serialization;
using Commerce.Query.Contract.Enumerations;
using Commerce.Query.Contract.Errors;

namespace Commerce.Query.Contract.Abstractions
{
    /// <summary>
    /// Represent application error
    /// </summary>
    [JsonDerivedType(typeof(StackTraceError))]
    [JsonDerivedType(typeof(Error))]
    public interface IError
    {
        /// <summary>
        /// Indicate which type of error
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ErrorType Type { get; }

        /// <summary>
        /// Code of error
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// Use for providing more information
        /// </summary>
        public List<string>? Details { get; }
    }
}