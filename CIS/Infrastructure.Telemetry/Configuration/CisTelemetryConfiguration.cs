namespace CIS.Core.Configuration;

public sealed class CisTelemetryConfiguration
    : ICisTelemetryConfiguration
{
    public Telemetry.ILoggingConfiguration? Logging { get; set; }
}
