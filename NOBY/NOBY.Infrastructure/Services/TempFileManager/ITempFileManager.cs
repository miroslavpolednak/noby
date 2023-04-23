using Microsoft.AspNetCore.Http;

namespace NOBY.Infrastructure.Services.TempFileManager;

public interface ITempFileManager
{
    public string ComposeFilePath(string fileName);

    public void CheckIfDocumentExist(string filePath);

    public Task<byte[]> GetDocument(string filePath, CancellationToken cancellationToken);

    public void BatchDelete(List<TempDocumentInformation>? attachments);

    public void BatchDelete(List<string> filePaths);

    public void CreateDirectoryIfNotExist(string directoryPath);

    public Task SaveFileToTempStorage(string path, IFormFile file, CancellationToken cancellationToken);

    Task<List<string>> UploadToArchive(long caseId, string? contractNumber, List<TempDocumentInformation> attachments, CancellationToken cancellationToken);
}

