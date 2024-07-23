using NOBY.Services.FileAntivirus;

namespace NOBY.Api.Endpoints.DocumentArchive.UploadDocument;

public class UploadDocumentHandler(
    SharedComponents.Storage.ITempStorage _tempFileManager, 
    IFileAntivirusService _fileAntivirus, 
    ILogger<UploadDocumentHandler> _logger) 
    : IRequestHandler<UploadDocumentRequest, Guid>
{
    public async Task<Guid> Handle(UploadDocumentRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Scanning file {Filename}", request.File.FileName);

        var antivirusResult = await _fileAntivirus.CheckFile(request.File);
        if (antivirusResult.Result != FileAntivirusResult.CheckFileResults.Passed)
        {
            _logger.LogWarning("Scanning file {Filename} failed with result: {Result}: {Message}", request.File.FileName, antivirusResult.Result, antivirusResult.ErrorMessage);
            throw new NobyValidationException(90037);
        }

        var result = await _tempFileManager.Save(request.File, cancellationToken: cancellationToken);
        return result.TempStorageItemId;
    }
}
