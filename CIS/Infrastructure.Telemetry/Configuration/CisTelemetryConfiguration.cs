namespace CIS.Infrastructure.Telemetry;

public sealed class CisTelemetryConfiguration
{
    public LoggingConfiguration? Logging { get; set; }

    public sealed class LoggingConfiguration
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
}
