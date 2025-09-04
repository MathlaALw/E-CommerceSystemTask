using E_CommerceSystem.Models;
using System.Text.Json;

namespace E_CommerceSystem.Helpers
{
    public class ErrorResponseHelper
    {
        public static async Task WriteErrorResponse(HttpResponse response, ErrorResponse errorResponse, bool indent = false)
        {
            response.ContentType = "application/json";
            response.StatusCode = errorResponse.StatusCode;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = indent
            };

            var json = JsonSerializer.Serialize(errorResponse, options);
            await response.WriteAsync(json);
        }
    }
}
