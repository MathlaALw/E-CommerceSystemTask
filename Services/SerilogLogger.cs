using Serilog;
namespace E_CommerceSystem.Services
{
    public class SerilogLogger<T> : IAppLogger<T>
    {
        private readonly ILogger _logger;

        public SerilogLogger(ILogger logger)
        {
            _logger = logger.ForContext<T>();
        }

        public void LogDebug(string message, params object[] args)
        {
            _logger.Debug(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            _logger.Error(message, args);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            _logger.Error(exception, message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.Information(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.Warning(message, args);
        }
    }

}

