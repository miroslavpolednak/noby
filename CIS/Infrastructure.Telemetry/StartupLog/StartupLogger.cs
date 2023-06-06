using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Reflection;

namespace CIS.Infrastructure.Telemetry.StartupLog;

internal sealed class StartupLogger
    : IStartupLogger
{
    private readonly Serilog.Core.Logger _logger;

    public StartupLogger(Serilog.Core.Logger logger)
    {
        _logger = logger;
    }

    public void RegisteringServices()
        => _logger.Debug("Registering services");

    public void ApplicationBuilt()
        => _logger.Debug("Registering services");

    public void ApplicationRun()
        => _logger.Debug("Registering services");

    public void ApplicationFinished()
        => _logger.Debug("Registering services");

    public void CatchedException(Exception? exception)
        => _logger.Error(exception, "Startup exception");

    public void CloseAndFlush()
    {
        if (_logger is not null)
            ((IDisposable)_logger).Dispose();
    }

    internal static IStartupLogger Create(WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Debug();

        // dotahnout konfiguraci
        var configuration = builder.Configuration
            .GetSection(LoggingExtensions._configurationTelemetryKey + ":Logging:Application")
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

        return new StartupLogger(logger.CreateLogger());
    }
}
