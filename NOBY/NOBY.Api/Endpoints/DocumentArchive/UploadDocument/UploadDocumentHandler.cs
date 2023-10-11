using NOBY.Services.FileAntivirus;
using NOBY.Services.TempFileManager;

namespace NOBY.Api.Endpoints.DocumentArchive.UploadDocument;

public class UploadDocumentHandler : IRequestHandler<UploadDocumentRequest, Guid>
{
    private readonly ITempFileManagerService _tempFileManager;
    private readonly IFileAntivirusService _fileAntivirus;

    public UploadDocumentHandler(ITempFileManagerService tempFileManager, IFileAntivirusService fileAntivirus)
    {
        _fileAntivirus = fileAntivirus;
        _tempFileManager = tempFileManager;
    }

    public async Task<Guid> Handle(UploadDocumentRequest request, CancellationToken cancellationToken)
    {
        var antivirusResult = await _fileAntivirus.CheckFile(request.File);
        if (antivirusResult != IFileAntivirusService.CheckFileResults.Passed)
        {
            throw new NobyValidationException(90037);
        }

        var result = await _tempFileManager.Save(request.File, cancellationToken);
        return result.TempFileId;
    }
}
