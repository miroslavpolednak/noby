﻿using SharedTypes.Enums;
using DomainServices.DocumentOnSAService.Clients;
using _Domain = DomainServices.DocumentOnSAService.Contracts;

namespace NOBY.Api.Endpoints.DocumentOnSA.SendDocumentPreview;

public class SendDocumentOnSAPreviewHandler : IRequestHandler<SendDocumentOnSAPreviewRequest>
{
    private readonly IDocumentOnSAServiceClient _documentOnSaService;

    public async Task Handle(SendDocumentOnSAPreviewRequest request, CancellationToken cancellationToken)
    {
        var documentsResponse = await _documentOnSaService.GetDocumentsOnSAList(request.SalesArrangementId, cancellationToken);
        var documentOnSA = documentsResponse.DocumentsOnSA.FirstOrDefault(d => d.DocumentOnSAId == request.DocumentOnSAId);

        if (documentOnSA is null)
        {
            throw new CisNotFoundException(NobyValidationException.DefaultExceptionCode, "DocumentOnSA does not exist on provided sales arrangement.");
        }

        var isElectronicAndWorkflow = documentOnSA is { SignatureTypeId: (int)SignatureTypes.Electronic, Source: _Domain.Source.Workflow };
        var isValidOrFinal = documentOnSA.IsValid || documentOnSA.IsFinal;

        if (isElectronicAndWorkflow && isValidOrFinal)
        {
            await _documentOnSaService.SendDocumentPreview(request.DocumentOnSAId, cancellationToken);
        }
        else
        {
            throw new NobyValidationException($"Invalid DocumentOnSA Id = {request.DocumentOnSAId}.");
        }
    }

    public SendDocumentOnSAPreviewHandler(IDocumentOnSAServiceClient documentOnSaService)
    {
        _documentOnSaService = documentOnSaService;
    }
}