namespace CIS.Infrastructure.Telemetry.Configuration;

public sealed class TracingConfiguration
{
    /// <summary>
    /// Logovat output do console
    /// </summary>
    public bool UseConsole { get; set; }

    public OtlpConfiguration? OTLP { get; set; }
}

public sealed class OtlpConfiguration
{
    public string? CollectorUrl { get; set; }
}
