using App.Errors;
using System.Net;
using System.Text.Json;

namespace App.MiddleWare
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        // Static to avoid creating new instance on every exception
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Check if response has already started
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Response has already started, cannot modify status code or headers");
                return;
            }

            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                // Custom exceptions
                NotFoundException => (HttpStatusCode.NotFound, exception.Message),
                UnauthorizedException => (HttpStatusCode.Unauthorized, exception.Message),
                ForbiddenException => (HttpStatusCode.Forbidden, exception.Message),
                BadRequestException => (HttpStatusCode.BadRequest, exception.Message),
                ValidationException => (HttpStatusCode.BadRequest, exception.Message),

                // Default
                _ => (HttpStatusCode.InternalServerError, "An internal server error occurred")
            };

            context.Response.StatusCode = (int)statusCode;

            var response = _env.IsDevelopment()
                ? new ApiException(
                    (int)statusCode,
                    message,
                    exception.StackTrace?.ToString() ?? "No stack trace available")
                : new ApiException((int)statusCode, message);

            var json = JsonSerializer.Serialize(response, JsonOptions);
            await context.Response.WriteAsync(json);
        }
    }


    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }

    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}
