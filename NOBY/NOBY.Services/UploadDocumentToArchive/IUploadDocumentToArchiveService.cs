namespace NOBY.Services.UploadDocumentToArchive;

public interface IUploadDocumentToArchiveService
{
    Task<List<string>> Upload(long caseId, string? contractNumber, List<DocumentMetadata> attachments, CancellationToken cancellationToken);
}
