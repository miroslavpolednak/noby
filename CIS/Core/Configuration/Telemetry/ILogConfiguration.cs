namespace CIS.Core.Configuration.Telemetry;

public interface ILogConfiguration
{
    IApplicationInsightsLogger? ApplicationInsights { get; set; }
    IMsSqlLogger? Database { get; set; }
    IFileLogger? File { get; set; }
    ISeqLogger? Seq { get; set; }
    bool LogRequestPayload { get; set; }
    bool LogResponsePayload { get; set; }
    bool UseConsole { get; set; }
}
