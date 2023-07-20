using DomainServices.RealEstateValuationService.Clients;
using NOBY.Infrastructure.Services.TempFileManager;

namespace NOBY.Api.Endpoints.RealEstateValuation.SaveRealEstateValuationAttachments;

internal sealed class SaveRealEstateValuationAttachmentsHandler
    : IRequestHandler<SaveRealEstateValuationAttachmentsRequest>
{
    public async Task Handle(SaveRealEstateValuationAttachmentsRequest request, CancellationToken cancellationToken)
    {
        foreach (var attachment in request.Attachments!)
        {
            var content = await _tempFileManager.GetContent(attachment.TempFileId, cancellationToken);
            var metadata = await _tempFileManager.GetMetadata(attachment.TempFileId, cancellationToken);
            
            var dsRequest = new DomainServices.RealEstateValuationService.Contracts.CreateRealEstateValuationAttachmentRequest
            {
                RealEstateValuationId = request.RealEstateValuationId,
                FileName = metadata!.FileName,
                MimeType = metadata.MimeType,
                Title = attachment.Title,
                FileData = Google.Protobuf.ByteString.CopyFrom(content)
            };

            await _realEstateValuationService.CreateRealEstateValuationAttachment(dsRequest, cancellationToken);
        }
    }

    private readonly ITempFileManagerService _tempFileManager;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public SaveRealEstateValuationAttachmentsHandler(IRealEstateValuationServiceClient realEstateValuationService, ITempFileManagerService tempFileManager)
    {
        _tempFileManager = tempFileManager;
        _realEstateValuationService = realEstateValuationService;
    }
}
