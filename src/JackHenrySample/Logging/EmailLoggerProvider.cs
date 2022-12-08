using JackHenrySample.Interfaces;

namespace JackHenrySample.Logging
{
    public class EmailLoggerProvider : ILoggerProvider
    {
        internal readonly IEmailService EmailService;

        public EmailLoggerProvider(IEmailService emailService)
        {
            EmailService = emailService;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new EmailLogger(this);
        }

        public void Dispose() { }
    }
}
