using SharedTypes.Enums;
using DomainServices.DocumentOnSAService.Clients;
using _Domain = DomainServices.DocumentOnSAService.Contracts;
using NOBY.Services.PermissionAccess;

namespace NOBY.Api.Endpoints.DocumentOnSA.SendDocumentPreview;

public class SendDocumentOnSAPreviewHandler : IRequestHandler<SendDocumentOnSAPreviewRequest>
{
    private readonly IDocumentOnSAServiceClient _documentOnSaService;
    private readonly INonWFLProductSalesArrangementAccess _nonWFLProductSalesArrangementAccess;

    public SendDocumentOnSAPreviewHandler(
       IDocumentOnSAServiceClient documentOnSaService,
       INonWFLProductSalesArrangementAccess nonWFLProductSalesArrangementAccess)
    {
        _documentOnSaService = documentOnSaService;
        _nonWFLProductSalesArrangementAccess = nonWFLProductSalesArrangementAccess;
    }

    public async Task Handle(SendDocumentOnSAPreviewRequest request, CancellationToken cancellationToken)
    {
        var documentsResponse = await _documentOnSaService.GetDocumentsOnSAList(request.SalesArrangementId, cancellationToken);
        var documentOnSA = documentsResponse.DocumentsOnSA.FirstOrDefault(d => d.DocumentOnSAId == request.DocumentOnSAId)
            ?? throw new CisNotFoundException(ErrorCodeMapper.DefaultExceptionCode, "DocumentOnSA does not exist on provided sales arrangement.");

        if (documentOnSA.Source != _Domain.Source.Workflow)
        {
            await _nonWFLProductSalesArrangementAccess.CheckNonWFLProductSalesArrangementAccess(documentOnSA.SalesArrangementId, cancellationToken);
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
}