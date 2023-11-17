namespace CIS.Infrastructure.Telemetry.Configuration;

public sealed class CisTelemetryConfiguration
{
    public LoggingConfiguration? Logging { get; set; }

    public TracingConfiguration? Tracing { get; set; }
}