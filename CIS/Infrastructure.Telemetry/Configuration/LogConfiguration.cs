namespace CIS.Infrastructure.Telemetry.Configuration;

public sealed class LogConfiguration
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
    /// Maximální velikost obsahu vlastnosti Payload
    /// </summary>
    public int? MaxPayloadLength { get; set; } = 1000000;

    /// <summary>
    /// Logovani do souboru
    /// </summary>
    public FileLogger? File { get; set; }

    /// <summary>
    /// Logovani do databaze
    /// </summary>
    public MsSqlLogger? Database { get; set; }

    /// <summary>
    /// Logovat output do console
    /// </summary>
    public bool UseConsole { get; set; }

    /// <summary>
    /// Logovani do Sequ
    /// </summary>
    public SeqLogger? Seq { get; set; }

    public ApplicationInsightsLogger? ApplicationInsights { get; set; }

    public sealed class SeqLogger
    {
        public string ServerUrl { get; set; } = "";
    }

    /// <summary>
    /// Nastaveni File sink dle https://github.com/serilog/serilog-sinks-file
    /// </summary>
    public sealed class FileLogger
    {
        public int RetainedFileCountLimit { get; set; } = 180;
        public long FileSizeLimitBytes { get; set; } = 512 * 1024 * 1024;
        public bool RollOnFileSizeLimit { get; set; } = true;
        public string Path { get; set; } = "";
        public string Filename { get; set; } = "";
    }

    public sealed class MsSqlLogger
    {
        public string ConnectionString { get; set; } = "";
    }

    public sealed class ApplicationInsightsLogger
    {
        public string ConnectionString { get; set; } = "";
    }
}
