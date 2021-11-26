using Serilog;
using System.Reflection;

namespace CIS.Infrastructure.StartupExtensions;

public static class AppLogging
{
    public static IHostBuilder UseAppLogging(this IHostBuilder builder)
    {
        builder.UseSerilog((hostingContext, serviceProvider, loggerConfiguration) =>
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var assembly = Assembly.GetEntryAssembly().GetName();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", $"{assembly.Name}")
                .Enrich.WithProperty("Version", $"{assembly.Version}");

            string logPath = Path.Combine(hostingContext.HostingEnvironment.ContentRootPath, "logs", "log.txt");
            loggerConfiguration
                .AuditTo.File(logPath)
                .WriteTo.Console();
        });
        return builder;
    }

    public static void CloseAndFlushLog()
    {
        Log.CloseAndFlush();
    }
}
