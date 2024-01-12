using Microsoft.AspNetCore.Http;

namespace SharedComponents.Storage;

public interface ITempStorage
{
    Task<TempStorageItem> Save(
        IFormFile file,
        long? objectId = null,
        string? objectType = null,
        Guid? sessionId = null,
        CancellationToken cancellationToken = default);

    Task<TempStorageItem> Save(
        IFormFile file,
        CancellationToken cancellationToken = default);

    Task<List<TempStorageItem>> GetSession(Guid sessionId, CancellationToken cancellationToken = default);

    Task<TempStorageItem> GetMetadata(Guid tempFileId, CancellationToken cancellationToken = default);

    Task<byte[]> GetContent(Guid tempFileId, CancellationToken cancellationToken = default);

    Task Delete(Guid tempFileId, CancellationToken cancellationToken = default);

    Task Delete(IEnumerable<Guid> tempFileId, CancellationToken cancellationToken = default);
}