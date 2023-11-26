using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace CIS.Infrastructure.Telemetry;

/// <summary>
/// Extension metody do startupu aplikace pro registraci logování.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Vytvoreni statickeho loggeru pro logovani startupu aplikace.
    /// </summary>
    public static IStartupLogger CreateStartupLogger(this WebApplicationBuilder builder)
    {
        return StartupLog.StartupLogger.Create(builder);
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
        builder
            .Services
            .AddOptions<LoggingConfiguration>()
            .Bind(builder.Configuration.GetSection(_configurationTelemetryKey));

        // pridani custom enricheru
        builder.Services.AddTransient<Enrichers.CisHeadersEnricher>();

        // pridani serilogu
        builder.Host.UseSerilog((hostingContext, serviceProvider, loggerConfiguration) =>
        {
            var configuration = serviceProvider.GetRequiredService<IOptions<LoggingConfiguration>>().Value;

            if (configuration is not null)
            {
                var bootstrapper = new LoggerBootstraper(hostingContext, serviceProvider, configuration);

                bootstrapper.SetupFilters(loggerConfiguration);

                // general log setup
                if (configuration?.Application is not null)
                {
                    bootstrapper.EnrichLogger(loggerConfiguration, configuration.Application);
                    bootstrapper.AddOutputs(loggerConfiguration, configuration.Application);
                }
            }
        });

        return builder;
    }

    /// <summary>
    /// pridani request behaviour mediatru - loguje request a response objekty
    /// logovat pouze u gRPC sluzeb
    /// </summary>
    public static WebApplicationBuilder AddCisLoggingPayloadBehavior(this WebApplicationBuilder builder) 
    {
        // logger
        builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(CisMediatR.PayloadLoggerBehavior<,>));

        // logger configuration
        builder.Services.AddSingleton(services =>
        {
            var configuration = services.GetRequiredService<IOptions<LoggingConfiguration>>().Value;

            return new CisMediatR.PayloadLogger.PayloadLoggerBehaviorConfiguration
            {
                LogRequestPayload = configuration!.Application?.LogRequestPayload ?? false,
                LogResponsePayload = configuration.Application?.LogResponsePayload ?? false
            };
        });

        return builder;
    }

    /// <summary>
    /// Pri ukonceni aplikaci se ujisti, ze vsechny sinky jsou vyprazdnene
    /// </summary>
    public static void CloseAndFlush()
    {
        StartupLog.StartupLogger.ApplicationFinished();

        Log.CloseAndFlush();
        StartupLog.StartupLogger.CloseAndFlush();

        // kdyz tu neni sleep, tak se obcas nezapsal vsechen output pri ukonceni sluzby
        Thread.Sleep(2000);
    }

    internal const string _configurationTelemetryKey = "CisTelemetry:Logging";
}
