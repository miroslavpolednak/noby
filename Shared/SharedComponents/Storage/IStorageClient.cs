namespace SharedComponents.Storage;

public interface IStorageClient<TStorage>
{
    Task SaveFile(byte[] data, string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default);

    Task<byte[]> GetFile(string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default);
}
