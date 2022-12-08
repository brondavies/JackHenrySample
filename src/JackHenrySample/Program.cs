using JackHenrySample.Logging;

namespace JackHenrySample
{
    public partial class Program
    {
        public static Task Main(string[] args)
        {
            return WebBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder WebBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureDefaultLogging()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.AddServerHeader = false;
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}