using DomainServices.DocumentOnSAService.Clients;

namespace NOBY.Api.Endpoints.DocumentOnSA.SendDocumentPreview;

public class SendDocumentPreviewHandler : IRequestHandler<SendDocumentPreviewRequest>
{
    private readonly IDocumentOnSAServiceClient _documentOnSaService;
    
    public async Task Handle(SendDocumentPreviewRequest request, CancellationToken cancellationToken)
    { 
        var documentsResponse = await _documentOnSaService.GetDocumentsOnSAList(request.SalesArrangementId, cancellationToken);
        var documentOnSA = documentsResponse.DocumentsOnSA.FirstOrDefault(d => d.DocumentOnSAId == request.DocumentOnSAId);

        if (documentOnSA is null)
        {
            throw new CisNotFoundException(90001, "DocumentOnSA does not exist on provided sales arrangement.");
        }

        var isElectronicAndWorkflow = documentOnSA.SignatureTypeId == 3; // && documentOnSA.Source == 2 ??
        var isValidOrFinal = documentOnSA.IsValid || documentOnSA.IsFinal;

        if (isElectronicAndWorkflow && isValidOrFinal)
        {
            // await _documentOnSaService.SendDocumentPreview()
        }

        throw new CisValidationException($"Invalid DocumentOnSA Id = {request.DocumentOnSAId}.");
    }

    public SendDocumentPreviewHandler(IDocumentOnSAServiceClient documentOnSaService)
    {
        _documentOnSaService = documentOnSaService;
    }
}