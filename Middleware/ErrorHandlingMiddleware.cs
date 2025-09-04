using E_CommerceSystem.Exceptions;
using E_CommerceSystem.Helpers;
using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
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
            string logMessage;
            LogLevel logLevel;

            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = exception.Message;
                    logMessage = $"Resource not found: {exception.Message}";
                    logLevel = LogLevel.Warning;
                    break;

                case BadRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    logMessage = $"Bad request: {exception.Message}";
                    logLevel = LogLevel.Warning;
                    break;

                case UnauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    logMessage = $"Unauthorized: {exception.Message}";
                    logLevel = LogLevel.Warning;
                    break;

                case ForbiddenException:
                    statusCode = HttpStatusCode.Forbidden;
                    message = exception.Message;
                    logMessage = $"Forbidden: {exception.Message}";
                    logLevel = LogLevel.Warning;
                    break;

                case ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    logMessage = $"Validation failed: {exception.Message}";
                    logLevel = LogLevel.Warning;

                    // Log validation errors in detail
                    foreach (var error in validationEx.Errors)
                    {
                        _logger.LogWarning("Validation error for {Field}: {Errors}",
                            error.Key, string.Join(", ", error.Value));
                    }
                    break;

                case AppException appEx when !appEx.IsOperational:
                    statusCode = (HttpStatusCode)appEx.StatusCode;
                    message = appEx.Message;
                    logMessage = $"Application error: {appEx.Message}";
                    logLevel = LogLevel.Error;
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "An internal server error has occurred.";
                    logMessage = $"Unhandled exception: {exception.Message}";
                    logLevel = LogLevel.Error;
                    break;
            }

            // Log the error with appropriate level
            switch (logLevel)
            {
                case LogLevel.Warning:
                    _logger.LogWarning(exception.Message , logMessage);
                    break;
                case LogLevel.Error:
                    _logger.LogError(exception, logMessage);
                    break;
                case LogLevel.Critical:
                    _logger.LogCritical(exception, logMessage);
                    break;
                default:
                    _logger.LogError(exception, logMessage);
                    break;
            }

            // Aadditional context to the log
            _logger.LogDebug("Error details - Status: {StatusCode}, Path: {Path}, Method: {Method}, User: {User}",
                (int)statusCode, context.Request.Path, context.Request.Method,
                context.User.Identity?.Name ?? "Anonymous");

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

