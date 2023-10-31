namespace CIS.Core.Configuration.Telemetry;

public interface IApplicationInsightsLogger
{
    string ConnectionString { get; }
}