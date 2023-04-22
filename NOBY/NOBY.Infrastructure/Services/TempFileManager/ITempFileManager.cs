namespace NOBY.Infrastructure.Services.TempFileManager;

public interface ITempFileManager
{
    public string ComposeFilePath(string fileName);

    public void CheckIfDocumentExist(string filePath);

    public Task<byte[]> GetDocument(string filePath, CancellationToken cancellationToken);

    public void BatchDelete(List<string> filePaths);

    void BatchDelete(List<TempDocumentInformation>? attachments);

    Task<List<string>> UploadToArchive(long caseId, string? contractNumber, List<TempDocumentInformation> attachments, CancellationToken cancellationToken);
}

