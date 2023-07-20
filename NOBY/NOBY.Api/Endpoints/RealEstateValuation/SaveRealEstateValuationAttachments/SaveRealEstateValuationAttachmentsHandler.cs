using DomainServices.RealEstateValuationService.Clients;
using NOBY.Infrastructure.Services.TempFileManager;

namespace NOBY.Api.Endpoints.RealEstateValuation.SaveRealEstateValuationAttachments;

internal sealed class SaveRealEstateValuationAttachmentsHandler
    : IRequestHandler<SaveRealEstateValuationAttachmentsRequest, List<SaveRealEstateValuationAttachmentsResponseItem>>
{
    public async Task<List<SaveRealEstateValuationAttachmentsResponseItem>> Handle(SaveRealEstateValuationAttachmentsRequest request, CancellationToken cancellationToken)
    {
        List<SaveRealEstateValuationAttachmentsResponseItem> newIds = new(request.Attachments!.Count);

        foreach (var attachment in request.Attachments)
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

            var id = await _realEstateValuationService.CreateRealEstateValuationAttachment(dsRequest, cancellationToken);
            newIds.Add(new()
            {
                TempFileId = attachment.TempFileId,
                RealEstateValuationAttachmentId = id,
            });

            await _tempFileManager.Delete(attachment.TempFileId, cancellationToken);
        }

        return newIds;
    }

    private readonly ITempFileManagerService _tempFileManager;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public SaveRealEstateValuationAttachmentsHandler(IRealEstateValuationServiceClient realEstateValuationService, ITempFileManagerService tempFileManager)
    {
        _tempFileManager = tempFileManager;
        _realEstateValuationService = realEstateValuationService;
    }
}
