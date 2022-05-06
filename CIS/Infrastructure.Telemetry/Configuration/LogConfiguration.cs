namespace CIS.Infrastructure.Telemetry.Configuration;

public sealed class LogConfiguration
{
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

    public sealed class SeqLogger
    {
        public string ServerUrl { get; set; } = "";
    }

    public sealed class FileLogger
    {
        public string Path { get; set; } = "";
        public string Filename { get; set; } = "";
    }

    public sealed class MsSqlLogger
    {
        public string ConnectionString { get; set; } = "";
    }
}
