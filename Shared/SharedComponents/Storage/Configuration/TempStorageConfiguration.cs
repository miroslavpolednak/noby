namespace SharedComponents.Storage.Configuration;

public sealed class TempStorageConfiguration
{
    public int MaxFileNameSize { get; set; } = 64;
    public string[]? AllowedFileExtensions { get; set; } = null!;
    public string ConnectionString { get; set; } = null!;
    public string StoragePath { get; set; } = null!;
}
