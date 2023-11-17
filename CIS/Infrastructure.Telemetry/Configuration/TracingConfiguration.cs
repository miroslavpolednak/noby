namespace CIS.Infrastructure.Telemetry.Configuration;

public sealed class TracingConfiguration
{
    public TracingProviders Provider { get; set; }

    public ConnectionConfiguration? Connection { get; set; }
}

public sealed class ConnectionConfiguration
{
    public string? Url { get; set; }
}

public enum TracingProviders
{
    None = 0,
    OpenTelemetry = 1
}