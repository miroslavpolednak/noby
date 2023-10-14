using NOBY.Services.FileAntivirus;
using NOBY.Services.TempFileManager;

namespace NOBY.Api.Endpoints.DocumentArchive.UploadDocument;

public class UploadDocumentHandler : IRequestHandler<UploadDocumentRequest, Guid>
{
    private readonly ITempFileManagerService _tempFileManager;
    private readonly IFileAntivirusService _fileAntivirus;
    private readonly ILogger<UploadDocumentHandler> _logger;

    public UploadDocumentHandler(ITempFileManagerService tempFileManager, IFileAntivirusService fileAntivirus, ILogger<UploadDocumentHandler> logger)
    {
        _fileAntivirus = fileAntivirus;
        _tempFileManager = tempFileManager;
        _logger = logger;
    }

    public async Task<Guid> Handle(UploadDocumentRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Scanning file {Filename}", request.File.FileName);

        var antivirusResult = await _fileAntivirus.CheckFile(request.File);
        if (antivirusResult.Result != FileAntivirusResult.CheckFileResults.Passed)
        {
            _logger.LogWarning("Scanning file {Filename} failed with result: {Result}: {Message}", request.File.FileName, antivirusResult.Result, antivirusResult.ErrorMessage);
            throw new NobyValidationException(90037);
        }

        var result = await _tempFileManager.Save(request.File, cancellationToken);
        return result.TempFileId;
    }
}
