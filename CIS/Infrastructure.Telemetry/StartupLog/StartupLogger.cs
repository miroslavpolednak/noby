using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Reflection;

namespace CIS.Infrastructure.Telemetry.StartupLog;

internal sealed class StartupLogger
    : IStartupLogger
{
    internal static Serilog.Core.Logger? _logger;

    public void ApplicationBuilt()
        => _logger!.Debug("STARTUP: Application built");

    public void ApplicationRun()
        => _logger!.Debug("STARTUP: Run");

    public void CatchedException(Exception? exception)
        => _logger!.Error(exception, "STARTUP: Exception");

    internal static void ApplicationStarting()
        => _logger!.Debug("STARTUP: Starting");

    internal static void ApplicationFinished()
        => _logger!.Debug("STARTUP: Finished");

    internal static void CloseAndFlush()
    {
        if (_logger is not null)
            ((IDisposable)_logger).Dispose();
    }

    private StartupLogger() 
    {
        ApplicationStarting();
    }

    internal static IStartupLogger Create(WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Debug();

        // dotahnout konfiguraci
        var configuration = builder.Configuration
            .GetSection(LoggingExtensions._configurationTelemetryKey + ":Logging:Application")
            .Get<LogConfiguration>()!;
        // env name
        var envName = builder.Configuration.GetValue<string>(Core.CisGlobalConstants.EnvironmentConfigurationSectionName + ":EnvironmentName") ?? "";

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var assemblyName = Assembly.GetEntryAssembly().GetName();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        logger
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Assembly", $"{assemblyName!.Name}")
            .Enrich.WithProperty("Version", $"{assemblyName!.Version}")
            .Enrich.WithProperty("CisEnvironment", envName);

        Helpers.AddOutputs(logger, configuration, null);

        _logger = logger.CreateLogger();
        
        return new StartupLogger();
    }
}
