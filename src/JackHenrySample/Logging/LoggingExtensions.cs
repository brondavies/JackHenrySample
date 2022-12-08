using JackHenrySample.Configuration;

namespace JackHenrySample.Logging
{
    public static class LoggingExtensions
    {
        public static IHostBuilder ConfigureDefaultLogging(this IHostBuilder builder)
        {
            return builder.ConfigureLogging(logging =>
            {
                logging.AddDebug().AddConsole();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    logging.AddEventLog();
                }
                if (!Global.Debug)
                {
                    //For production environment errors
                    logging.AddEmailLogger();
                }
            });
        }

        public static ILoggingBuilder AddEmailLogger(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, EmailLoggerProvider>();
            return builder;
        }
    }
}
