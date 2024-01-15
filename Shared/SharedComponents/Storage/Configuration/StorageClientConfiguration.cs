namespace SharedComponents.Storage.Configuration;

public sealed class StorageClientConfiguration
{
    public StorageClientTypes StorageType { get; set; }

    public string ConnectionStringOrPath { get; set; } = null!;

    public string? Container {  get; set; }

}

public enum StorageClientTypes
{
    FileSystem = 1,
    AzureBlob = 2,
    S3 = 3
}