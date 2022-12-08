using JackHenrySample.Implementation;
using JackHenrySample.Interfaces;

namespace JackHenrySample
{
    internal class Dependencies
    {
        internal static void Build(IServiceCollection services, bool console = false)
        {
            services.AddTransient<ITwitterStreamService, TwitterSampleStreamService>();
            services.AddSingleton<IEmailService, StubEmailService>();
            services.AddLogging();
        }
    }
}
