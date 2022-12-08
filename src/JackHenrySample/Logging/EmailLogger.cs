using JackHenrySample.Configuration;

namespace JackHenrySample.Logging
{
    public class EmailLogger : ILogger
    {
        protected readonly EmailLoggerProvider _emailLoggerProvider;

        public EmailLogger(EmailLoggerProvider emailLoggerProvider)
        {
            _emailLoggerProvider = emailLoggerProvider;
        }

#pragma warning disable CS8603
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
#pragma warning restore CS8603

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Error;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel) || Global.Debug)
            {
                return;
            }

            var logRecord = string.Format("{0}<br><pre>{1}</pre><br>{2}, {3}, {4}", exception?.Message, exception != null ? exception.StackTrace : "no stack trace", logLevel, eventId, state);
            _emailLoggerProvider.EmailService.SendEmail("errors@example.com", "no-reply@example.com", "Error from web app", logRecord);
        }
    }
}
