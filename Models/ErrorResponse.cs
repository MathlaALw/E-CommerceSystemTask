using Azure.Core;
using E_CommerceSystem.Exceptions;

namespace E_CommerceSystem.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
        public string Type { get; set; }
        public string RequestId { get; set; }
        public DateTime Timestamp { get; set; }

        public ErrorResponse(Exception ex, bool includeStackTrace = false)
        {
            StatusCode = ex is AppException appEx ? appEx.StatusCode : 500;
            Message = ex.Message;
            Type = ex.GetType().Name;
            RequestId = Guid.NewGuid().ToString();
            Timestamp = DateTime.UtcNow;

            if (includeStackTrace)
            {
                StackTrace = ex.ToString();
            }

            if (ex is ValidationException validationEx)
            {
                Errors = validationEx.Errors;
            }
        }
    }
}
