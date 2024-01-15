using Azure.Storage.Blobs;
using SharedComponents.Storage.Configuration;

namespace SharedComponents.Storage.StorageClients;

internal sealed class AzureBlobStorageClient<TStorage>
    : IStorageClient<TStorage>
{
    public async Task<byte[]> GetFile(string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        var result = await getBlobContainerClient(ref folderOrContainer).GetBlobClient(fileName).DownloadContentAsync(cancellationToken);
        return result.Value.Content.ToArray();
    }

    public async Task SaveFile(byte[] data, string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        await getBlobContainerClient(ref folderOrContainer).UploadBlobAsync(fileName, new BinaryData(data), cancellationToken);
    }

    private BlobContainerClient getBlobContainerClient(ref string? container)
    {
        var blobServiceClient = new BlobServiceClient(_configuration.AzureBlob!.ConnectionString);
        return blobServiceClient.GetBlobContainerClient(string.IsNullOrEmpty(container) ? "$root" : container);
    }

    private readonly StorageClientConfiguration _configuration;

    public AzureBlobStorageClient(StorageClientConfiguration configuration)
    {
        if (string.IsNullOrEmpty(configuration.FileSystem?.BasePath))
        {
            throw new CisConfigurationException(0, $"AzureBlobStorageClient configuration error: ConnectionString for client '{typeof(TStorage).Name}' is missing");
        }

        _configuration = configuration;
    }
}
