using System.Globalization;
using System.Net.Mime;
using CIS.Core;
using DomainServices.CodebookService.Clients;
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
    private readonly ICodebookServiceClient _codebookService;
    private readonly IDateTime _dateTime;

    public GetElectronicDocumentPreviewHandler(
        DocumentOnSAServiceDbContext dbContext,
        IESignaturesClient eSignaturesClient,
        ICodebookServiceClient codebookService,
        IDateTime dateTime)
    {
        _dbContext = dbContext;
        _eSignaturesClient = eSignaturesClient;
        _codebookService = codebookService;
        _dateTime = dateTime;
    }

    public async Task<GetElectronicDocumentPreviewResponse> Handle(GetElectronicDocumentPreviewRequest request, CancellationToken cancellationToken)
    {
        var documentOnSA = await _dbContext.DocumentOnSa
            .Where(r => r.DocumentOnSAId == request.DocumentOnSAId)
            .FirstOrDefaultAsync(cancellationToken)
        ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist);

        var documentPreviewData = await _eSignaturesClient.DownloadDocumentPreview(documentOnSA.ExternalId ?? string.Empty, cancellationToken);
        
        var templates = await _codebookService.DocumentTypes(cancellationToken);
        var fileName = templates.First(t => t.Id == documentOnSA.DocumentTypeId).FileName;
        
        return new GetElectronicDocumentPreviewResponse
        {
            Filename = $"{fileName}_{documentOnSA.DocumentOnSAId}_{_dateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}.pdf",
            MimeType = MediaTypeNames.Application.Pdf,
            BinaryData = ByteString.CopyFrom(documentPreviewData)
        };
    }
}
