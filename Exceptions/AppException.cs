namespace E_CommerceSystem.Exceptions
{
   
        public class AppException : Exception
        {
            public int StatusCode { get; set; }
            public bool IsOperational { get; set; }

            public AppException(string message, int statusCode = 500, bool isOperational = true)
                : base(message)
            {
                StatusCode = statusCode;
                IsOperational = isOperational;
            }
        }

        public class NotFoundException : AppException
        {
            public NotFoundException(string message)
                : base(message, 404) { }
        }

        public class BadRequestException : AppException
        {
            public BadRequestException(string message)
                : base(message, 400) { }
        }

        public class UnauthorizedException : AppException
        {
            public UnauthorizedException(string message)
                : base(message, 401) { }
        }

        public class ForbiddenException : AppException
        {
            public ForbiddenException(string message)
                : base(message, 403) { }
        }

        public class ValidationException : AppException
        {
            public Dictionary<string, string[]> Errors { get; set; }

            public ValidationException(Dictionary<string, string[]> errors)
                : base("Validation failed", 400)
            {
                Errors = errors;
            }
        }
    }
