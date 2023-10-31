using CIS.Core.Configuration.Telemetry;

namespace CIS.Infrastructure.Telemetry.Configuration;

public sealed class LogConfiguration
    : ILogConfiguration
{
    /// <summary>
    /// True = do logu se ulozi plny request payload sluzby
    /// </summary>
    public bool LogRequestPayload { get; set; } = true;

    /// <summary>
    /// True = do logu se ulozi plny response payload sluzby
    /// </summary>
    public bool LogResponsePayload { get; set; } = true;

    /// <summary>
    /// Logovani do souboru
    /// </summary>
    public IFileLogger? File { get; set; }

    /// <summary>
    /// Logovani do databaze
    /// </summary>
    public IMsSqlLogger? Database { get; set; }

    /// <summary>
    /// Logovat output do console
    /// </summary>
    public bool UseConsole { get; set; }

    /// <summary>
    /// Logovani do Sequ
    /// </summary>
    public ISeqLogger? Seq { get; set; }

    public IApplicationInsightsLogger? ApplicationInsights { get; set; }

    public sealed class SeqLogger : ISeqLogger
    {
        public string ServerUrl { get; set; } = "";
    }

    /// <summary>
    /// Nastaveni File sink dle https://github.com/serilog/serilog-sinks-file
    /// </summary>
    public sealed class FileLogger : IFileLogger
    {
        public int RetainedFileCountLimit { get; set; } = 180;
        public long FileSizeLimitBytes { get; set; } = 512 * 1024 * 1024;
        public bool RollOnFileSizeLimit { get; set; } = true;
        public string Path { get; set; } = "";
        public string Filename { get; set; } = "";
    }

    public sealed class MsSqlLogger : IMsSqlLogger
    {
        public string ConnectionString { get; set; } = "";
    }

    public sealed class ApplicationInsightsLogger : IApplicationInsightsLogger
    {
        public string ConnectionString { get; set; } = "";
    }
}
