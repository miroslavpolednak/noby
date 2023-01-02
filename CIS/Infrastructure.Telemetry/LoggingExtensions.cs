using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CIS.Infrastructure.Telemetry;

/// <summary>
/// Extension metody do startupu aplikace pro registraci logování.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Podle nastavení v appsettings.json zařazuje middleware pro logování buď gRPC nebo Webapi.
    /// </summary>
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

    /// <summary>
    /// Přidává do aplikace logování pomocí Serilogu.
    /// </summary>
    /// <remarks>
    /// Načte konfiguraci logování z appsettings.json.
    /// Přidá do DI IAuditLogger pro auditní logování.
    /// Přidá logování request a response do MediatR pipeline.
    /// </remarks>
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
        builder.Host.AddCisLoggingInternal();

        // pridani request behaviour mediatru - loguje request a response objekty
        // logovat pouze u gRPC sluzeb
        if (configuration.Logging?.LogType != LogBehaviourTypes.WebApi)
            builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(CisMediatR.PayloadLoggerBehavior<,>));

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

    private static IHostBuilder AddCisLoggingInternal(this IHostBuilder builder)
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
