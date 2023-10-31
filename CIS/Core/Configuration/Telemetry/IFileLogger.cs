namespace CIS.Core.Configuration.Telemetry;

public interface IFileLogger
{
    public int RetainedFileCountLimit { get; }
    public long FileSizeLimitBytes { get; }
    public bool RollOnFileSizeLimit { get; }
    public string Path { get; }
    public string Filename { get; }
}
