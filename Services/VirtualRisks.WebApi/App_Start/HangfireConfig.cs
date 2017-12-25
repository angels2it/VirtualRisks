using Hangfire;
using Hangfire.Console;
using Hangfire.Mongo;
using Owin;

namespace CastleGo.WebApi
{
    internal static class HangfireConfig
    {
        public static IAppBuilder UseHangfire(this IAppBuilder app)
        {
            Hangfire.GlobalConfiguration.Configuration
                .UseConsole()
                .UseMongoStorage(System.Configuration.ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString, "castlego");
            app.UseHangfireDashboard();
            app.UseHangfireServer();
            return app;
        }
    }
}