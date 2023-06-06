using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;

namespace CIS.Infrastructure.Telemetry;

/// <summary>
/// Extension metody do startupu aplikace pro registraci logování.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Vytvoreni statickeho loggeru pro logovani startupu aplikace.
    /// </summary>
    public static Serilog.Core.Logger CreateStartupLogger(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Debug();

        // dotahnout konfiguraci
        var configuration = builder.Configuration
            .GetSection(_configurationTelemetryKey + ":Logging:Application")
            .Get<LogConfiguration>()!;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var assemblyName = Assembly.GetEntryAssembly().GetName();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        logger
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Assembly", $"{assemblyName!.Name}")
            .Enrich.WithProperty("Version", $"{assemblyName!.Version}");

        Helpers.AddOutputs(logger, configuration, null);
        
        return logger.CreateLogger();
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
        var configuration = builder.Configuration
            .GetSection(_configurationTelemetryKey)
            .Get<CisTelemetryConfiguration>()
            ?? throw new CIS.Core.Exceptions.CisConfigurationNotFound(_configurationTelemetryKey);
        builder.Services.AddSingleton(configuration);

        // pridani custom enricheru
        builder.Services.AddTransient<Enrichers.NobyHeadersEnricher>();

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
