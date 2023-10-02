using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using ExternalServices.SbQueues.V1;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetElectronicDocumentFromQueue;

public class GetElectronicDocumentFromQueueHandler : IRequestHandler<GetElectronicDocumentFromQueueRequest, GetElectronicDocumentFromQueueResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly ISbQueuesRepository _signatureQueues;

    public GetElectronicDocumentFromQueueHandler(
        DocumentOnSAServiceDbContext dbContext,
        ISbQueuesRepository signatureQueues)
    {
        _dbContext = dbContext;
        _signatureQueues = signatureQueues;
    }

    public async Task<GetElectronicDocumentFromQueueResponse> Handle(GetElectronicDocumentFromQueueRequest request, CancellationToken cancellationToken)
    {
        return request.EDocumentCase switch
        {
            GetElectronicDocumentFromQueueRequest.EDocumentOneofCase.MainDocument
                => await HandleMainDocument(request.MainDocument?.DocumentId ?? string.Empty, cancellationToken),
            GetElectronicDocumentFromQueueRequest.EDocumentOneofCase.DocumentAttachment
                => await HandleDocumentAttachment(request.DocumentAttachment?.AttachmentId ?? string.Empty, cancellationToken),
            _ => throw new NotSupportedException(),
        };
    }

    private async Task<GetElectronicDocumentFromQueueResponse> HandleMainDocument(string documentId, CancellationToken cancellationToken)
    {
        var documentFile = await _signatureQueues.GetDocumentByExternalId(documentId, cancellationToken);
        if (documentFile?.Content == null || !documentFile.Content.Any())
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentFileNotExist, documentId);
        }

        return new()
        {
            Filename = documentFile.FileName,
            MimeType = documentFile.ContentType,
            BinaryData = ByteString.CopyFrom(documentFile.Content)
        };
    }

    private async Task<GetElectronicDocumentFromQueueResponse> HandleDocumentAttachment(string attachmentId, CancellationToken cancellationToken)
    {
        var attachmentFile = await _signatureQueues.GetAttachmentById(attachmentId, cancellationToken);
        if (attachmentFile?.Content == null || !attachmentFile.Content.Any())
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.AttachmentFileNotExist, attachmentId);
        }

        return new()
        {
            Filename = attachmentFile.FileName,
            MimeType = attachmentFile.ContentType,
            BinaryData = ByteString.CopyFrom(attachmentFile.Content)
        };
    }
}
