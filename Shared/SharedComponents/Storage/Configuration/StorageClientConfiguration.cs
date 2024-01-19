namespace SharedComponents.Storage.Configuration;

public sealed class StorageClientConfiguration
{
    public StorageClientTypes StorageType { get; set; }

    public StorageClientConfigurationFileSystem? FileSystem { get; set; }
    public StorageClientConfigurationAzureBlob? AzureBlob { get; set; }
    public StorageClientConfigurationAmazonS3? AmazonS3 { get; set; }

    public sealed class StorageClientConfigurationFileSystem
    {
        public string BasePath { get; set; } = null!;
    }

    public sealed class StorageClientConfigurationAzureBlob
    {
        public string ConnectionString { get; set; } = null!;
    }

    public sealed class StorageClientConfigurationAmazonS3
    {
        public string ServiceUrl { get; set; } = null!;
        public string AccessKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public string Bucket { get; set; } = null!;
        public int RetentionPeriod { get; set; }
    }
}

public enum StorageClientTypes
{
    None = 0,
    FileSystem = 1,
    AzureBlob = 2,
    AmazonS3 = 3
}