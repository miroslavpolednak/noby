using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using ExternalServices.ESignatures.V1;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetElectronicDocumentPreview;

public class GetElectronicDocumentPreviewHandler : IRequestHandler<GetElectronicDocumentPreviewRequest, GetElectronicDocumentPreviewResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IESignaturesClient _eSignaturesClient;

    public GetElectronicDocumentPreviewHandler(
        DocumentOnSAServiceDbContext dbContext,
        IESignaturesClient eSignaturesClient)
    {
        _dbContext = dbContext;
        _eSignaturesClient = eSignaturesClient;
    }

    public async Task<GetElectronicDocumentPreviewResponse> Handle(GetElectronicDocumentPreviewRequest request, CancellationToken cancellationToken)
    {
        var externalId = await _dbContext.DocumentOnSa
            .Where(r => r.DocumentOnSAId == request.DocumentOnSAId)
            .Select(r => r.ExternalId)
            .FirstOrDefaultAsync(cancellationToken)
        ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist);

        var documentPreviewData = await _eSignaturesClient.DownloadDocumentPreview(externalId, cancellationToken);
        
        return new GetElectronicDocumentPreviewResponse
        {
            Filename = "doc.pdf",
            MimeType = "application/pdf",
            BinaryData = ByteString.CopyFrom(documentPreviewData)
        };
    }
}
