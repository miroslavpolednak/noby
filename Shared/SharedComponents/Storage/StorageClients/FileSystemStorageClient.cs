using SharedComponents.Storage.Configuration;

namespace SharedComponents.Storage.StorageClients;

internal sealed class FileSystemStorageClient<TStorage>
    : IStorageClient<TStorage>
{
    public async Task<byte[]> GetFile(string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        string path = Path.Combine(_configuration.FileSystem!.BasePath, folderOrContainer ?? "", fileName);
        return await File.ReadAllBytesAsync(path, cancellationToken);
    }

    public async Task SaveFile(byte[] data, string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        string path = Path.Combine(_configuration.FileSystem!.BasePath, folderOrContainer ?? "", fileName);
        await File.WriteAllBytesAsync(path, data, cancellationToken);
    }

    private readonly StorageClientConfiguration _configuration;

    public FileSystemStorageClient(StorageClientConfiguration configuration)
    {
        if (string.IsNullOrEmpty(configuration.FileSystem?.BasePath))
        {
            throw new CisConfigurationException(0, $"FileSystemStorageClient configuration error: BasePath for client '{typeof(TStorage).Name}' is missing");
        }

        _configuration = configuration;
    }
}
