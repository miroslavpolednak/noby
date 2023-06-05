using NOBY.Infrastructure.Configuration;
using NOBY.Infrastructure.Services.TempFileManager;

namespace NOBY.Api.Endpoints.DocumentArchive.UploadDocument;

public class UploadDocumentHandler : IRequestHandler<UploadDocumentRequest, Guid>
{
    private readonly AppConfiguration _configuration;
    private readonly ITempFileManager _tempFileManager;

    public UploadDocumentHandler(AppConfiguration configuration, ITempFileManager tempFileManager)
    {
        _configuration = configuration;
        _tempFileManager = tempFileManager;
    }

    public async Task<Guid> Handle(UploadDocumentRequest request, CancellationToken cancellationToken)
    {
        _tempFileManager.CreateDirectoryIfNotExist(_configuration.FileTempFolderLocation);

        var tempFilename = Guid.NewGuid();
        var fullPath = Path.Combine(_configuration.FileTempFolderLocation, tempFilename.ToString());

        await _tempFileManager.SaveFileToTempStorage(fullPath, request.File, cancellationToken);

        return tempFilename;
    }
}
