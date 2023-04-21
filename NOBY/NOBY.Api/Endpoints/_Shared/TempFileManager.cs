using CIS.Core.Attributes;
using NOBY.Infrastructure.Configuration;
using System.Threading;

namespace NOBY.Api.Endpoints.Shared;

public interface ITempFileManager
{
    public string ComposeFilePath(string fileName);

    public void CheckIfDocumentExist(string filePath);

    public Task<byte[]> GetDocument(string filePath, CancellationToken cancellationToken);

    public void BatchDelete(List<string> filePaths);

    public void CreateDirectoryIfNotExist(string directoryPath);

    public Task SaveFileToTempStorage(string path, IFormFile file, CancellationToken cancellationToken);
}

[ScopedService, AsImplementedInterfacesService]
public class TempFileManager: ITempFileManager
{
    private readonly AppConfiguration _configuration;

    public TempFileManager(AppConfiguration configuration)
	{
        _configuration = configuration;
    }

    public void BatchDelete(List<string> filePaths)
    {
        filePaths.ForEach(File.Delete);
    }

    public void CreateDirectoryIfNotExist(string directoryPath)
    {
        if (!Directory.Exists(_configuration.FileTempFolderLocation))
            Directory.CreateDirectory(_configuration.FileTempFolderLocation);
    }

    public void CheckIfDocumentExist(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new CisNotFoundException(250, $"Document not found on temp storage");
        }
    }

    public string ComposeFilePath(string fileName)
    {
        return Path.Combine(_configuration.FileTempFolderLocation, fileName);
    }

    public async Task<byte[]> GetDocument(string filePath, CancellationToken cancellationToken)
    {
       return  await File.ReadAllBytesAsync(filePath, cancellationToken);
    }

    public async Task SaveFileToTempStorage(string path, IFormFile file, CancellationToken cancellationToken)
    {
        using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);
        await stream.FlushAsync(cancellationToken);
    }
}
