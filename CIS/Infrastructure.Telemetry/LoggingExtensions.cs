using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CIS.Infrastructure.Telemetry;

public static class LoggingExtensions
{
    public static IApplicationBuilder UseCisLogging(this IApplicationBuilder webApplication)
    {
        webApplication.UseMiddleware<Middlewares.LoggerCisUserMiddleware>();
        return webApplication;
    }

    public static WebApplicationBuilder AddCisLogging(this WebApplicationBuilder builder)
    {
        // auditni log
        builder.Services.AddSingleton<IAuditLogger>(new AuditLogger());

        builder.Host.AddCisLogging();
        
        return builder;
    }

    private static IHostBuilder AddCisLogging(this IHostBuilder builder)
    {
        builder.UseSerilog((hostingContext, serviceProvider, loggerConfiguration) =>
        {
            // get configuration from json file
            var configSection = hostingContext.Configuration.GetSection("CisTelemetry");
            CisTelemetryConfiguration _configuration = new();
            configSection.Bind(_configuration);

            var bootstrapper = new LoggerBootstraper(hostingContext, serviceProvider);

            // general log setup
            if (_configuration?.Logging?.Application is not null)
            {
                bootstrapper.EnrichLogger(loggerConfiguration);
                bootstrapper.AddOutputs(loggerConfiguration, _configuration.Logging.Application);
            }

            // audit log setup
            if (_configuration?.Logging?.Audit is not null)
            {
                AuditLogger.SetupLogger(bootstrapper, _configuration.Logging.Audit);
            }
        });

        return builder;
    }

    public static void CloseAndFlush()
    {
        Log.CloseAndFlush();
        AuditLogger.CloseAndFlush();

        Thread.Sleep(2000);
    }
}
