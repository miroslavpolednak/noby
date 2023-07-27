using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using ExternalServices.ESignatureQueues.V1;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetElectronicDocumentFromQueue;

public class GetElectronicDocumentFromQueueHandler : IRequestHandler<GetElectronicDocumentFromQueueRequest, GetElectronicDocumentFromQueueResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IESignatureQueuesRepository _signatureQueues;

    public GetElectronicDocumentFromQueueHandler(
        DocumentOnSAServiceDbContext dbContext,
        IESignatureQueuesRepository signatureQueues)
    {
        _dbContext = dbContext;
        _signatureQueues = signatureQueues;
    }

    public async Task<GetElectronicDocumentFromQueueResponse> Handle(GetElectronicDocumentFromQueueRequest request, CancellationToken cancellationToken)
    {
        return request.EDocumentCase switch
        {
            GetElectronicDocumentFromQueueRequest.EDocumentOneofCase.MainDocument
                => await HandleMainDocument(request.MainDocument?.DocumentOnSAId ?? 0, cancellationToken),
            GetElectronicDocumentFromQueueRequest.EDocumentOneofCase.DocumentAttachment
                => await HandleDocumentAttachment(request.DocumentAttachment?.DocumentAttachmentId ?? string.Empty, cancellationToken),
            _ => throw new NotSupportedException(),
        };
    }

    private async Task<GetElectronicDocumentFromQueueResponse> HandleMainDocument(int documentOnSAId, CancellationToken cancellationToken)
    {
        var externalId = await _dbContext.DocumentOnSa
            .Where(r => r.DocumentOnSAId == documentOnSAId)
            .Select(r => r.ExternalId)
            .FirstOrDefaultAsync(cancellationToken)
        ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist, documentOnSAId);
        
        var documentFile = await _signatureQueues.GetDocumentByExternalId(externalId, cancellationToken);
        if (documentFile?.Content == null || !documentFile.Content.Any())
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentFileNotExist);
        }
        
        return new()
        {
            Filename = documentFile.Name,
            MimeType = documentFile.ContentType,
            BinaryData = ByteString.CopyFrom(documentFile.Content)
        };
    }

    private async Task<GetElectronicDocumentFromQueueResponse> HandleDocumentAttachment(string documentAttachmentId, CancellationToken cancellationToken)
    {
        // confirmed with IT ANA
        if (!long.TryParse(documentAttachmentId, out var attachmentId))
        {
            throw ErrorCodeMapper.CreateNotFoundException(0);
        }
        
        var attachmentFile = await _signatureQueues.GetAttachmentById(attachmentId, cancellationToken);
        if (attachmentFile?.Content == null || !attachmentFile.Content.Any())
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.AttachmentFileNotExist);
        }

        return new()
        {
            Filename = attachmentFile.Name,
            MimeType = attachmentFile.ContentType,
            BinaryData = ByteString.CopyFrom(attachmentFile.Content)
        };
    }
}
