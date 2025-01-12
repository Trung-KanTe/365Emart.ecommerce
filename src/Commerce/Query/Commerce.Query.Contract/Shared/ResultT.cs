using Commerce.Query.Contract.Abstractions;
using Commerce.Query.Contract.Enumerations;
using Commerce.Query.Contract.Errors;

namespace Commerce.Query.Contract.Shared
{
    /// <summary>
    /// Provide result with specific type of data for use case handle
    /// </summary>
    public class Result<TModel> : IResult
    {
        public bool IsSuccess { get; }
        public int StatusCode { get; }
        public string? Message { get; }
        public IError? Error { get; }

        /// <summary>
        /// Value of result if any
        /// </summary>
        public TModel? Data { get; private set; }

        /// <summary>
        /// Provide result with specific type of data for use case handle
        /// </summary>
        public Result(bool isSuccess,
                      int statusCode,
                      string? message = null,
                      IError? error = null,
                      TModel? data = default)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = message;
            Error = error;
            Data = data;
        }

        /// <summary>
        /// Implicit cast from model to generic result
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Result<TModel>(TModel value)
        {
            return new Result<TModel>(true, 200, data: value);
        }

        /// <summary>
        /// Implicit cast from result to generic result
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Result<TModel>(Result value)
        {
            return new Result<TModel>(value.IsSuccess, value.StatusCode, value.Message, value.Error);
        }

        /// <summary>
        /// Implicit cast from error to result
        /// </summary>
        /// <param name="error"></param>
        public static implicit operator Result<TModel>(Error error)
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
        public static implicit operator Result<TModel>(StackTraceError error)
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