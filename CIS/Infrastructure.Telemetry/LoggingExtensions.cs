using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CIS.Infrastructure.Telemetry;

public static class LoggingExtensions
{
    public static IApplicationBuilder UseCisLogging(this IApplicationBuilder webApplication)
    {
        webApplication.UseWhen(
            httpContext => httpContext.RequestServices.GetRequiredService<CisTelemetryConfiguration>().Logging?.LogType == LogBehaviourTypes.WebApi, 
            builder => builder.UseMiddleware<Middlewares.LoggerCisUserWebapiMiddleware>());

        webApplication.UseWhen(
            httpContext => httpContext.RequestServices.GetRequiredService<CisTelemetryConfiguration>().Logging?.LogType == LogBehaviourTypes.Grpc,
            builder => builder.UseMiddleware<Middlewares.LoggerCisUserGrpcMiddleware>());
        
        return webApplication;
    }

    public static WebApplicationBuilder AddCisLogging(this WebApplicationBuilder builder)
    {
        // get configuration from json file
        var configSection = builder.Configuration.GetSection(_configurationTelemetryKey);
        CisTelemetryConfiguration configuration = new();
        configSection.Bind(configuration);
        builder.Services.AddSingleton(configuration);

        // auditni log
        builder.Services.AddSingleton<IAuditLogger>(new AuditLogger());

        // pridani serilogu
        builder.Host.AddCisLogging();

        // pridani request behaviour mediatru - loguje request a response objekty
        builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(Mediatr.LoggingBehaviour<,>));

        return builder;
    }

    /// <summary>
    /// Pri ukonceni aplikaci se ujisti, ze vsechny sinky jsou vyprazdnene
    /// </summary>
    public static void CloseAndFlush()
    {
        Log.CloseAndFlush();
        AuditLogger.CloseAndFlush();

        // kdyz tu neni sleep, tak se obcas nezapsal vsechen output pri ukonceni sluzby
        Thread.Sleep(2000);
    }

    private static IHostBuilder AddCisLogging(this IHostBuilder builder)
    {
        builder.UseSerilog((hostingContext, serviceProvider, loggerConfiguration) =>
        {
            var configuration = serviceProvider.GetRequiredService<CisTelemetryConfiguration>();

            if (configuration.Logging is not null)
            {
                var bootstrapper = new LoggerBootstraper(hostingContext, serviceProvider, configuration.Logging.LogType);

                bootstrapper.SetupFilters(loggerConfiguration);

                // general log setup
                if (configuration?.Logging?.Application is not null)
                {
                    bootstrapper.EnrichLogger(loggerConfiguration);
                    bootstrapper.AddOutputs(loggerConfiguration, configuration.Logging.Application);
                }

                // audit log setup
                if (configuration?.Logging?.Audit is not null)
                {
                    AuditLogger.SetupLogger(bootstrapper, configuration.Logging.Audit);
                }
            }
        });

        return builder;
    }

    const string _configurationTelemetryKey = "CisTelemetry";
}
