using System.Net.Mime;
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

        // todo:
        return CreateMockResponse();
    }

    private async Task<GetElectronicDocumentFromQueueResponse> HandleDocumentAttachment(string documentAttachmentId, CancellationToken cancellationToken)
    {
        // todo:
        await _signatureQueues.GetAttachmentExternalId(documentAttachmentId, cancellationToken);
        return CreateMockResponse();
    }
    
    private static GetElectronicDocumentFromQueueResponse CreateMockResponse()
    {
        return new GetElectronicDocumentFromQueueResponse
        {
            Filename = "doc.pdf",
            MimeType = MediaTypeNames.Application.Pdf,
            BinaryData = ByteString.CopyFrom(Convert.FromBase64String("""JVBERi0yLjAKJbq63toKNSAwIG9iajw8L0xpbmVhcml6ZWQgMS9MIDE3NzcvTyA4L0UgMTMxMC9OIDEvVCAxNDkwL0ggWyA1OTIgMzAwXT4+CmVuZG9iagogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAo2IDAgb2JqPDwvUm9vdCA3IDAgUi9JbmZvIDMgMCBSL0lEWzxDQTlCNUU2NEQzRjJBNTc2MzdBRkRBRUMxRTU2NDI5QT48NEM5NjE3QjA3OThDQThERjZFRDZBNjk0Q0RCM0EzQUY+XS9TaXplIDEzL1ByZXYgMTQ5MS9MZW5ndGggMzkvVHlwZS9YUmVmL0ZpbHRlci9GbGF0ZURlY29kZS9EZWNvZGVQYXJtczw8L0NvbHVtbnMgMy9QcmVkaWN0b3IgMTI+Pi9JbmRleFs1IDhdL1dbMSAyIDBdPj5zdHJlYW0KeJxjYmTgZ2JgOMHEwOTLxMAYAcRtTIx/2Jn+s7QxMfw7CABAawZkCmVuZHN0cmVhbQplbmRvYmoKc3RhcnR4cmVmCjAKJSVFT0YKICAgICAgICAgICAgICAgICAgICAgICAgICAgIAo3IDAgb2JqPDwvVHlwZS9DYXRhbG9nL1BhZ2VzIDIgMCBSPj4KZW5kb2JqCjEyIDAgb2JqPDwvUyAzNi9GaWx0ZXIvRmxhdGVEZWNvZGUvTGVuZ3RoIDQyPj5zdHJlYW0KeJxjYGBgZmBgCmAAAsZFDHAAZTMBMQtCFKQWjBkY2hhY2CcwAABQXALJCmVuZHN0cmVhbQplbmRvYmoKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCjggMCBvYmo8PC9UeXBlL1BhZ2UvUGFyZW50IDIgMCBSL01lZGlhQm94WzAgMCA2MTIgNzkyXS9SZXNvdXJjZXM8PC9Gb250PDwvRjAgMTAgMCBSPj4+Pi9Dcm9wQm94WzAgMCA2MTIgNzkyXS9Db250ZW50cyAxMSAwIFI+PgplbmRvYmoKOSAwIG9iajw8L1R5cGUvT2JqU3RtL04gMS9GaXJzdCA1L0ZpbHRlci9GbGF0ZURlY29kZS9MZW5ndGggNTI+PnN0cmVhbQp4nDM0UDBQsLHRD6ksSNV3y88r0Q8uTSoBcUAihvpOicWpYGGP1Jyy1JLM5EQ7OwCykhF/CmVuZHN0cmVhbQplbmRvYmoKMTEgMCBvYmo8PC9GaWx0ZXIvRmxhdGVEZWNvZGUvTGVuZ3RoIDc3Pj5zdHJlYW0KeJwr5DJQAMGidCgjyJ3LKYTLyFTB3MxMz8zUwMDIxEQhJIVL381AwdBIISSNSyMkI7NYQTMki0sDRidCqJLU4hIQwzWEK5ALAGkWFIQKZW5kc3RyZWFtCmVuZG9iagoxIDAgb2JqPDwvVHlwZS9PYmpTdG0vTiAyL0ZpcnN0IDkvRmlsdGVyL0ZsYXRlRGVjb2RlL0xlbmd0aCA5Mj4+c3RyZWFtCnicM1IwUDBWMDZVsLHRD6ksSNUPSExPLdb3zkwpjrYAygXF6jvnl+aVKBja2QGVOBelJpZk5ue5JJakarhYGRkYGRuYGRoZGpoYGxtGaer75qfgkLKzAwBCtBzGCmVuZHN0cmVhbQplbmRvYmoKNCAwIG9iajw8L1Jvb3QgNyAwIFIvSW5mbyAzIDAgUi9JRFs8Q0E5QjVFNjREM0YyQTU3NjM3QUZEQUVDMUU1NjQyOUE+PDRDOTYxN0IwNzk4Q0E4REY2RUQ2QTY5NENEQjNBM0FGPl0vU2l6ZSA1L0xlbmd0aCAzMC9UeXBlL1hSZWYvRmlsdGVyL0ZsYXRlRGVjb2RlL0RlY29kZVBhcm1zPDwvQ29sdW1ucyA1L1ByZWRpY3RvciAxMj4+L0luZGV4WzAgNV0vV1sxIDIgMl0+PnN0cmVhbQp4nGNiAAEmRlY5EPn7MZAECTAy/We9xPAfAClmBOQKZW5kc3RyZWFtCmVuZG9iagpzdGFydHhyZWYKMjE1CiUlRU9GCg==""")),
        };
    }
}
