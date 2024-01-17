using SharedComponents.Storage.Configuration;

namespace SharedComponents.Storage.StorageClients;

internal sealed class FileSystemStorageClient<TStorage>
    : IStorageClient<TStorage>
{
    public async Task<byte[]> GetFile(string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        return await File.ReadAllBytesAsync(getPath(folderOrContainer ?? "", fileName), cancellationToken);
    }

    public async Task SaveFile(Stream data, string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        using (var stream = new FileStream(getPath(folderOrContainer ?? "", fileName), FileMode.Create))
        {
            await data.CopyToAsync(stream, cancellationToken);
            await stream.FlushAsync(cancellationToken);
        }
    }

    public async Task SaveFile(byte[] data, string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        await File.WriteAllBytesAsync(getPath(folderOrContainer ?? "", fileName), data, cancellationToken);
    }

    public Task DeleteFile(string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        File.Delete(getPath(folderOrContainer ?? "", fileName));
        return Task.CompletedTask;
    }

    private string getPath(ReadOnlySpan<char> folderOrContainer, ReadOnlySpan<char> fileName)
    {
        return Path.Combine(_configuration.FileSystem!.BasePath, folderOrContainer.ToString(), fileName.ToString());
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
