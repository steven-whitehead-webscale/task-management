using Microsoft.Extensions.Logging;

namespace TaskManagement.Infrastructure.Logging
{
    public interface ILoggingService
    {
        void LogInformation(string message);
        void LogError(string message, Exception ex);
    }

    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogError(string message, Exception ex)
        {
            _logger.LogError(ex, message);
        }
    }
} 