﻿using System.Globalization;
using System.Net.Mime;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using ExternalServices.ESignatures.V1;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetElectronicDocumentPreview;

public sealed class GetElectronicDocumentPreviewHandler : IRequestHandler<GetElectronicDocumentPreviewRequest, GetElectronicDocumentPreviewResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IESignaturesClient _eSignaturesClient;
    private readonly ICodebookServiceClient _codebookService;
    private readonly TimeProvider _dateTime;

    public GetElectronicDocumentPreviewHandler(
        DocumentOnSAServiceDbContext dbContext,
        IESignaturesClient eSignaturesClient,
        ICodebookServiceClient codebookService,
        TimeProvider dateTime)
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
            .Select(s => new
            {
                s.ExternalIdESignatures,
                s.DocumentTypeId,
                s.DocumentOnSAId
            })
            .FirstOrDefaultAsync(cancellationToken)
        ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist);

        var documentPreviewData = await _eSignaturesClient.DownloadDocumentPreview(documentOnSA.ExternalIdESignatures ?? string.Empty, cancellationToken);

        var templates = await _codebookService.DocumentTypes(cancellationToken);
        var fileName = templates.Find(t => t.Id == documentOnSA.DocumentTypeId)?.FileName;

        return new GetElectronicDocumentPreviewResponse
        {
            Filename = $"{fileName ?? "WFDocument"}_{documentOnSA.DocumentOnSAId}_{_dateTime.GetLocalNow().ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}.pdf",
            MimeType = MediaTypeNames.Application.Pdf,
            BinaryData = ByteString.CopyFrom(documentPreviewData)
        };
    }
}
