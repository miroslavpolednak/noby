using NOBY.Infrastructure.Services.TempFileManager;

namespace NOBY.Infrastructure.Services.UploadDocumentToArchive;

public interface IUploadDocumentToArchiveService
{
    Task<List<string>> Upload(long caseId, string? contractNumber, List<DocumentMetadata> attachments, CancellationToken cancellationToken);
}
