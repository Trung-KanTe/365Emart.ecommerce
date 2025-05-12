using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Exceptions;
using Commerce.Command.Contract.Shared;
using Microsoft.AspNetCore.Diagnostics;

namespace Commerce.Command.API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> logger;
        private readonly IWebHostEnvironment env;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context,
                                                    Exception exception,
                                                    CancellationToken cancellationToken)
        {
            logger.LogError(
                "Error Message: {exceptionMessage}, Time of occurrence {time}",
                exception.Message, DateTime.UtcNow);
            // Check current environment is production or not (can be development, QA, Test)
            var isProduction = env.IsProduction();
            // Base message of exception
            var message = "Error occured";
            // Make Result base on exception type
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
            // Set response status code synchronous with result status code
            context.Response.StatusCode = result.StatusCode;
            // Write result to response as application/json format
            await context.Response.WriteAsJsonAsync(result, cancellationToken);
            // Return true mark that exception has been handled and not go to other routes
            return true;
        }
    }
}