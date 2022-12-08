using JackHenrySample.Configuration;
using JackHenrySample.Services;
using System.Reflection;

namespace JackHenrySample
{
    public class Startup
    {
#pragma warning disable CS8618
        public static IConfiguration Configuration { get; private set; }
#pragma warning restore CS8618

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Global.Settings = new StandardSettings(configuration.AsDictionary());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSignalR();

            Dependencies.Build(services);

            services.AddHostedService<TwitterStreamConsumerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePagesWithReExecute("/Status", "?code={0}");
            if (env.IsDevelopment())
            {
                Global.Debug = true;
                app.UseDeveloperExceptionPage();
                //using (var services = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                //{
                //    services.ServiceProvider.GetService<ApplicationDataContext>().Database.Migrate();
                //}
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                MapSignalRHubs(endpoints);
            });

            app.UseResponseCaching();
        }

        static void MapSignalRHubs(IEndpointRouteBuilder routes)
        {
            var hubs = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace?.StartsWith("JackHenrySample.Hubs") == true && t.Name.EndsWith("Hub"));
            foreach (var hub in hubs)
            {
                hub?.GetMethod("MapHub", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, new object[] { routes });
            }
        }
    }
}
