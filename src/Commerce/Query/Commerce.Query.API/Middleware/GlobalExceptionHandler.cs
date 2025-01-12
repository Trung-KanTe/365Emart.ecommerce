using Commerce.Query.Contract.Contants;
using Commerce.Query.Contract.Enumerations;
using Commerce.Query.Contract.Errors;
using Commerce.Query.Contract.Exceptions;
using Commerce.Query.Contract.Shared;
using Microsoft.AspNetCore.Diagnostics;

namespace Commerce.Query.API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context,
                                                    Exception exception,
                                                    CancellationToken cancellationToken)
        {
            _logger.LogError(
                "Error Message: {exceptionMessage}, Time of occurrence {time}",
                exception.Message, DateTime.UtcNow);
            var isProduction = _env.IsProduction();
            var message = "Error occured";
            var result = exception switch
            {
                DomainValidationException validationException => new Result
                (
                    false,
                    StatusCode.BadRequest,
                    message,
                    isProduction
                        ? new Error(ErrorType.ValidationProblem, ErrCodeConst.VALIDATION_PROBLEM,
                            validationException.Details.ToArray())
                        : new StackTraceError(ErrorType.ValidationProblem, ErrCodeConst.VALIDATION_PROBLEM,
                            exception.StackTrace!,
                            validationException.Details.ToArray())
                ),
                NotFoundException notFoundException => new Result
                (
                    false,
                    StatusCode.NotFound,
                    message,
                    isProduction
                        ? new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, notFoundException.Message)
                        : new StackTraceError(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, exception.StackTrace!,
                            notFoundException.Message)
                ),
                ConflictException conflictException => new Result
                (
                    false,
                    StatusCode.Conflict,
                    message,
                    isProduction
                        ? new Error(ErrorType.Conflict, ErrCodeConst.CONFLICT, conflictException.Message)
                        : new StackTraceError(ErrorType.Conflict, ErrCodeConst.CONFLICT, exception.StackTrace!,
                            conflictException.Message)
                ),
                _ => new Result
                (
                    false,
                    StatusCode.InternalServerError,
                    message,
                    isProduction
                        ? new Error(ErrorType.ServerError, ErrCodeConst.INTERNAL_SERVER_ERROR)
                        : new StackTraceError(ErrorType.ServerError, ErrCodeConst.INTERNAL_SERVER_ERROR,
                            exception.StackTrace!, exception.Message)
                )
            };
            context.Response.StatusCode = result.StatusCode;
            await context.Response.WriteAsJsonAsync(result, cancellationToken);
            return true;
        }
    }
}