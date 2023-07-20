using NOBY.Infrastructure.Services.TempFileManager;

namespace NOBY.Api.Endpoints.RealEstateValuation.CreateRealEstateValuationAttachment;

internal sealed class UploadRealEstateValuationAttachmentHandler
    : IRequestHandler<UploadRealEstateValuationAttachmentRequest, Guid>
{
    public async Task<Guid> Handle(UploadRealEstateValuationAttachmentRequest request, CancellationToken cancellationToken)
    {
        var response = await _tempFileManager.Save(request.File, cancellationToken);
        return response.TempFileId;
    }

    private readonly ITempFileManagerService _tempFileManager;

    public UploadRealEstateValuationAttachmentHandler(ITempFileManagerService tempFileManager)
    {
        _tempFileManager = tempFileManager;
    }
}
