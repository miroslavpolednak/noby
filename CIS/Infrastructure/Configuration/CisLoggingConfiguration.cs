namespace CIS.Infrastructure.Configuration;

public sealed class CisLoggingConfiguration
{
    /// <summary>
    /// Logovani do souboru
    /// </summary>
    public FileLogger? File { get; set; }

    /// <summary>
    /// Logovani do databaze
    /// </summary>
    public MsSqlLogger? Database { get; set; }

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
