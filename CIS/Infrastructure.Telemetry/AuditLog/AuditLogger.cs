using Serilog;

namespace CIS.Infrastructure.Telemetry;

internal sealed class AuditLogger
    : IAuditLogger
{
    static Serilog.Core.Logger? _logger;

    public void Log(string message)
    {
        if (_logger is not null)
            _logger.Information(message);
    }

    internal static void SetupLogger(LoggerBootstraper bootstrapper, LogConfiguration configuration)
    {
        var logger = new LoggerConfiguration();

        bootstrapper.EnrichLogger(logger);
        bootstrapper.AddOutputs(logger, configuration);

        _logger = logger.CreateLogger();
    }

    internal static void CloseAndFlush()
    {
        if (_logger is not null)
            ((IDisposable)_logger).Dispose();
    }
}
