﻿using DomainServices.RealEstateValuationService.Clients;

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

            if (content.Length > _maxAllowedFileSize * 1024 * 1024)
                throw new NobyValidationException($"Max allowed file size of {_maxAllowedFileSize} MB was exceeded");

            // docitam to jednotlive, neni to nijak efektivni, ale vetsinou se budou stejne uploadovat jeden souboru...
            var metadata = await _tempFileManager.GetMetadata(attachment.TempFileId, cancellationToken);

            var dsRequest = new DomainServices.RealEstateValuationService.Contracts.CreateRealEstateValuationAttachmentRequest
            {
                RealEstateValuationId = request.RealEstateValuationId,
                FileName = metadata!.FileName,
                MimeType = metadata.MimeType,
                Title = attachment.Title ?? "",
                FileData = Google.Protobuf.ByteString.CopyFrom(content),
                AcvAttachmentCategoryId = attachment.AcvAttachmentCategoryId
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

    private readonly SharedComponents.Storage.ITempStorage _tempFileManager;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;
    /// <summary>
    /// [MB]
    /// </summary>
    private const int _maxAllowedFileSize = 8;
    public SaveRealEstateValuationAttachmentsHandler(IRealEstateValuationServiceClient realEstateValuationService, SharedComponents.Storage.ITempStorage tempFileManager)
    {
        _tempFileManager = tempFileManager;
        _realEstateValuationService = realEstateValuationService;
    }
}
