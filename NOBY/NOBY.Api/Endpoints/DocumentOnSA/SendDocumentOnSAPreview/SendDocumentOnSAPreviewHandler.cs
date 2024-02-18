using DomainServices.DocumentOnSAService.Clients;
using _Domain = DomainServices.DocumentOnSAService.Contracts;
using NOBY.Services.CheckNonWFLProductSalesArrangementAccess;

namespace NOBY.Api.Endpoints.DocumentOnSA.SendDocumentOnSAPreview;

public class SendDocumentOnSAPreviewHandler : IRequestHandler<SendDocumentOnSAPreviewRequest>
{
    private readonly Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService _salesArrangementAuthorization;
    private readonly IDocumentOnSAServiceClient _documentOnSaService;
    private readonly INonWFLProductSalesArrangementAccessService _nonWFLProductSalesArrangementAccess;

    public SendDocumentOnSAPreviewHandler(
       IDocumentOnSAServiceClient documentOnSaService,
       INonWFLProductSalesArrangementAccessService nonWFLProductSalesArrangementAccess,
       Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService salesArrangementAuthorization)
    {
        _documentOnSaService = documentOnSaService;
        _nonWFLProductSalesArrangementAccess = nonWFLProductSalesArrangementAccess;
        _salesArrangementAuthorization = salesArrangementAuthorization;
    }

    public async Task Handle(SendDocumentOnSAPreviewRequest request, CancellationToken cancellationToken)
    {
        // validace opravneni
        await _salesArrangementAuthorization.ValidateDocumentSigningMngBySaType237And246BySAId(request.SalesArrangementId, cancellationToken);

        var documentsResponse = await _documentOnSaService.GetDocumentsOnSAList(request.SalesArrangementId, cancellationToken);
        var documentOnSA = documentsResponse.DocumentsOnSA.FirstOrDefault(d => d.DocumentOnSAId == request.DocumentOnSAId)
            ?? throw new CisNotFoundException(ErrorCodeMapper.DefaultExceptionCode, "DocumentOnSA does not exist on provided sales arrangement.");

        if (documentOnSA.Source != _Domain.Source.Workflow)
        {
            await _nonWFLProductSalesArrangementAccess.CheckNonWFLProductSalesArrangementAccess(documentOnSA.SalesArrangementId, cancellationToken);
        }

        var isElectronicAndWorkflow = documentOnSA is { SignatureTypeId: (int)SignatureTypes.Electronic };
        var isValidOrFinal = documentOnSA.IsValid && !documentOnSA.IsFinal && !documentOnSA.IsSigned;

        if (isElectronicAndWorkflow && isValidOrFinal)
        {
            await _documentOnSaService.SendDocumentPreview(request.DocumentOnSAId, cancellationToken);
        }
        else
        {
            throw new NobyValidationException($"Invalid DocumentOnSA Id = {request.DocumentOnSAId}.");
        }
    }
}