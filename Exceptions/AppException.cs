namespace E_CommerceSystem.Exceptions
{
   
        public class AppException : Exception
        {
            public int StatusCode { get; set; }
            public bool IsOperational { get; set; }
            public string ErrorCode { get; set; }
        public AppException(string message, int statusCode = 500, bool isOperational = true, string errorCode = null)
      : base(message)
        {
            StatusCode = statusCode;
            IsOperational = isOperational;
            ErrorCode = errorCode ?? $"ERR_{statusCode}";
        }
    }

    public class NotFoundException : AppException
    {
        public NotFoundException(string resourceName, object resourceId)
            : base($"Resource '{resourceName}' with ID '{resourceId}' not found.", 404, true, "RESOURCE_NOT_FOUND") { }
    }

    public class BadRequestException : AppException
    {
        public BadRequestException(string message, string errorCode = "BAD_REQUEST")
            : base(message, 400, true, errorCode) { }
    }

    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message = "Unauthorized access")
            : base(message, 401, true, "UNAUTHORIZED") { }
    }

    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message = "Access forbidden")
            : base(message, 403, true, "FORBIDDEN") { }
    }

    public class ValidationException : AppException
    {
        public Dictionary<string, string[]> Errors { get; set; }

        public ValidationException(Dictionary<string, string[]> errors)
            : base("Validation failed", 400, true, "VALIDATION_ERROR")
        {
            Errors = errors;
        }
    }
}
