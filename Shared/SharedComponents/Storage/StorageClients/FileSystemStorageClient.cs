using SharedComponents.Storage.Configuration;

namespace SharedComponents.Storage.StorageClients;

internal sealed class FileSystemStorageClient
    : IStorageClient
{
    public async Task<byte[]> GetFile(string fileName, string folderOrContainer, CancellationToken cancellationToken = default)
    {
        string path = Path.Combine(_configuration.ConnectionStringOrPath, folderOrContainer ?? "", fileName);
        return await File.ReadAllBytesAsync(path, cancellationToken);
    }

    public async Task SaveFile(byte[] data, string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        string path = Path.Combine(_configuration.ConnectionStringOrPath, folderOrContainer ?? "", fileName);
        await File.WriteAllBytesAsync(path, data, cancellationToken);
    }

    private readonly StorageClientConfiguration _configuration;

    public FileSystemStorageClient(StorageClientConfiguration configuration)
    {
        _configuration = configuration;
    }
}
