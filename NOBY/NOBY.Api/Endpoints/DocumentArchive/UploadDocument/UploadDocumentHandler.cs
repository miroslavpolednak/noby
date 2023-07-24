using NOBY.Infrastructure.Services.TempFileManager;

namespace NOBY.Api.Endpoints.DocumentArchive.UploadDocument;

public class UploadDocumentHandler : IRequestHandler<UploadDocumentRequest, Guid>
{
    private readonly ITempFileManagerService _tempFileManager;

    public UploadDocumentHandler(ITempFileManagerService tempFileManager)
    {
        _tempFileManager = tempFileManager;
    }

    public async Task<Guid> Handle(UploadDocumentRequest request, CancellationToken cancellationToken)
    {
        var result = await _tempFileManager.Save(request.File, cancellationToken);
        return result.TempFileId;
    }
}
