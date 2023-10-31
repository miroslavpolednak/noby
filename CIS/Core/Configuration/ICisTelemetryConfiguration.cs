namespace CIS.Core.Configuration;

public enum LogBehaviourTypes
{
    Any,
    WebApi,
    Grpc
}

public interface ICisTelemetryConfiguration
{
    Telemetry.ILoggingConfiguration? Logging { get; }
}
