using Commerce.Query.Contract.Abstractions;
using Commerce.Query.Contract.Enumerations;
using Commerce.Query.Contract.Errors;

namespace Commerce.Query.Contract.Shared
{
    /// <summary>
    /// Provide result for use case handle
    /// </summary>
    public class Result : IResult
    {
        public bool IsSuccess { get; }
        public int StatusCode { get; }
        public string? Message { get; }
        public IError? Error { get; }

        public Result(bool isSuccess, int statusCode, string? message = null, IError? error = null)
        {
            // Throw an exception if the parameters are inconsistent
            if ((isSuccess && error is not null) ||
                (!isSuccess && error is null))
                throw new ArgumentException("Invalid error", nameof(error));
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = message;
            Error = error;
        }

        /// <summary>
        /// Create failure result with failed state and attached error 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="error">Error of failure</param>
        /// <returns>Failure result with error</returns>
        public static Result Failure(int statusCode, IError error)
        {
            return new Result(false, statusCode, error: error);
        }

        /// <summary>
        /// Create success result with success state
        /// </summary>
        /// <returns>Success result without data</returns>
        public static Result Ok()
        {
            return new Result(true, 200);
        }

        /// <summary>
        /// Create success result with data 
        /// </summary>
        /// <typeparam name="TModel">Generic type</typeparam>
        /// <param name="value"></param>
        /// <returns>Success result contain data</returns>
        public static Result<TModel> Ok<TModel>(TModel value)
        {
            return new Result<TModel>(true, 200, data: value);
        }

        /// <summary>
        /// Create failure result with failed state, attached error and data
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="statusCode"></param>
        /// <param name="value"></param>
        /// <param name="error"></param>
        /// <returns>A failed Result object with the given value and error</returns>
        public static Result<TModel> Failure<TModel>(int statusCode, TModel value, IError error)
        {
            return new Result<TModel>(true, statusCode, error: error, data: value);
        }

        /// <summary>
        /// Implicit cast from error to result
        /// </summary>
        /// <param name="error"></param>
        public static implicit operator Result(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.NotFound => Enumerations.StatusCode.NotFound,
                ErrorType.Conflict => Enumerations.StatusCode.Conflict,
                ErrorType.ServerError => Enumerations.StatusCode.InternalServerError,
                ErrorType.ValidationProblem => Enumerations.StatusCode.BadRequest,
                _ => throw new ArgumentOutOfRangeException()
            };
            return new Result(false, statusCode, null, error);
        }

        /// <summary>
        /// Implicit cast from stack trace error to result
        /// </summary>
        /// <param name="error"></param>
        public static implicit operator Result(StackTraceError error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.NotFound => Enumerations.StatusCode.NotFound,
                ErrorType.Conflict => Enumerations.StatusCode.Conflict,
                ErrorType.ServerError => Enumerations.StatusCode.InternalServerError,
                ErrorType.ValidationProblem => Enumerations.StatusCode.BadRequest,
                _ => throw new ArgumentOutOfRangeException()
            };
            return new Result(false, statusCode, null, error);
        }
    }
}