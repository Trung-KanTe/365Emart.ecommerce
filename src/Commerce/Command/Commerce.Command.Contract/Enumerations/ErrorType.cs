using System.Text.Json.Serialization;

namespace Commerce.Command.Contract.Enumerations
{
    /// <summary>
    /// Error type of domain error
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorType
    {
        /// <summary>
        /// Resources are not found
        /// </summary>
        NotFound,

        /// <summary>
        /// Fail to execute an action
        /// </summary>
        Failure,

        /// <summary>
        /// Resources conflict with other
        /// </summary>
        Conflict,

        /// <summary>
        /// Problem happen when validate resources
        /// </summary>
        ValidationProblem,

        /// <summary>
        /// Unknown exception cause internal server error
        /// </summary>
        ServerError,

        /// <summary>
        /// Value is null
        /// </summary>
        NullValue,

        /// <summary>
        /// Error is none
        /// </summary>
        None
    }
}