using Microsoft.AspNetCore.Http;

namespace NOBY.Infrastructure.Services.TempFileManager;

public interface ITempFileManagerService
{
    Task<TempFile> Save(
        IFormFile file,
        long? objectId = null,
        string? objectType = null,
        Guid? sessionId = null,
        CancellationToken cancellationToken = default);

    Task<TempFile> Save(
        IFormFile file,
        CancellationToken cancellationToken = default);

    Task<TempFile> GetMetadata(Guid tempFileId, CancellationToken cancellationToken = default);

    Task<byte[]> GetContent(Guid tempFileId, CancellationToken cancellationToken = default);

    Task Delete(Guid tempFileId, CancellationToken cancellationToken = default);

    Task Delete(IEnumerable<Guid> tempFileId, CancellationToken cancellationToken = default);
}