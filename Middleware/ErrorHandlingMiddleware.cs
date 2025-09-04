using E_CommerceSystem.Exceptions;
using E_CommerceSystem.Models;
using System.Net;
using System.Text.Json;

namespace E_CommerceSystem.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppLogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ErrorHandlingMiddleware(RequestDelegate next, IAppLogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            bool includeStackTrace = _environment.IsDevelopment();
            string message;

            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = exception.Message;
                    _logger.LogWarning(exception, "Resource not found: {Message}", exception.Message);
                    break;

                case BadRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    _logger.LogWarning(exception, "Bad request: {Message}", exception.Message);
                    break;

                case UnauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    _logger.LogWarning(exception, "Unauthorized: {Message}", exception.Message);
                    break;

                case ForbiddenException:
                    statusCode = HttpStatusCode.Forbidden;
                    message = exception.Message;
                    _logger.LogWarning(exception, "Forbidden: {Message}", exception.Message);
                    break;

                case ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    _logger.LogWarning(exception, "Validation failed: {Message}", exception.Message);
                    break;

                case AppException appEx when !appEx.IsOperational:
                    statusCode = (HttpStatusCode)appEx.StatusCode;
                    message = appEx.Message;
                    _logger.LogError(exception, "Application error: {Message}", exception.Message);
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "An internal server error has occurred.";
                    _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new ErrorResponse(exception, includeStackTrace);

            // Don't expose internal error details in production
            if (!includeStackTrace && statusCode == HttpStatusCode.InternalServerError)
            {
                errorResponse.Message = "An internal server error has occurred.";
                errorResponse.StackTrace = null;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _environment.IsDevelopment()
            };

            var json = JsonSerializer.Serialize(errorResponse, options);
            await context.Response.WriteAsync(json);
        }
    }
}
