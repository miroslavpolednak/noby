using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.DocumentOnSAService.ExternalServices.SbQueues.V1.Repositories;
using Google.Protobuf;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetElectronicDocumentFromQueue;

public class GetElectronicDocumentFromQueueHandler : IRequestHandler<GetElectronicDocumentFromQueueRequest, GetElectronicDocumentFromQueueResponse>
{
    private readonly ISbQueuesRepository _signatureQueues;

    public GetElectronicDocumentFromQueueHandler(
        ISbQueuesRepository signatureQueues)
    {
        _signatureQueues = signatureQueues;
    }

    public async Task<GetElectronicDocumentFromQueueResponse> Handle(GetElectronicDocumentFromQueueRequest request, CancellationToken cancellationToken)
    {
        return request.EDocumentCase switch
        {
            GetElectronicDocumentFromQueueRequest.EDocumentOneofCase.MainDocument
                => await HandleMainDocument(request.MainDocument?.DocumentId ?? string.Empty, request.GetMetadataOnly, cancellationToken),
            GetElectronicDocumentFromQueueRequest.EDocumentOneofCase.DocumentAttachment
                => await HandleDocumentAttachment(request.DocumentAttachment?.AttachmentId ?? string.Empty, request.GetMetadataOnly, cancellationToken),
            _ => throw new NotSupportedException(),
        };
    }

    private async Task<GetElectronicDocumentFromQueueResponse> HandleMainDocument(string documentId, bool getMetadataOnly, CancellationToken cancellationToken)
    {
        var documentFile = await _signatureQueues.GetDocumentByExternalId(documentId, getMetadataOnly, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentFileNotExist, documentId);

        return new()
        {
            Filename = documentFile.FileName,
            MimeType = documentFile.ContentType,
            BinaryData = documentFile.Content is not null ? ByteString.CopyFrom(documentFile.Content) : ByteString.Empty,
            IsCustomerPreviewSendingAllowed = documentFile.IsCustomerPreviewSendingAllowed,
            ExternalIdESignatures = documentFile.ExternalIdESignatures,
        };
    }

    private async Task<GetElectronicDocumentFromQueueResponse> HandleDocumentAttachment(string attachmentId, bool getMetadataOnly, CancellationToken cancellationToken)
    {
        var attachmentFile = await _signatureQueues.GetAttachmentById(attachmentId, getMetadataOnly, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.AttachmentFileNotExist, attachmentId);

        return new()
        {
            Filename = attachmentFile.FileName,
            MimeType = attachmentFile.ContentType,
            BinaryData = attachmentFile.Content is not null ? ByteString.CopyFrom(attachmentFile.Content) : ByteString.Empty,
            IsCustomerPreviewSendingAllowed = attachmentFile.IsCustomerPreviewSendingAllowed
        };
    }
}
