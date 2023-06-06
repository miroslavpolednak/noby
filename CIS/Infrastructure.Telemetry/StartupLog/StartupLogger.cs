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
    {
        _logger.Debug("Registering services");
    }

    public void CloseAndFlush()
    {
        if (_logger is not null)
            ((IDisposable)_logger).Dispose();
    }
}
