using DomainServices.DocumentOnSAService.Clients;
using FastEnumUtility;
using NOBY.Services.CheckNonWFLProductSalesArrangementAccess;

namespace NOBY.Api.Endpoints.DocumentOnSA.SignDocumentManually;

internal sealed class SignDocumentManuallyHandler : IRequestHandler<SignDocumentManuallyRequest>
{
    private readonly Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService _salesArrangementAuthorization;
    private readonly IDocumentOnSAServiceClient _documentOnSaClient;
    private readonly INonWFLProductSalesArrangementAccessService _nonWFLProductSalesArrangementAccess;

    public SignDocumentManuallyHandler(
        IDocumentOnSAServiceClient documentOnSaClient,
        INonWFLProductSalesArrangementAccessService nonWFLProductSalesArrangementAccess,
        Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService salesArrangementAuthorization)
    {
        _documentOnSaClient = documentOnSaClient;
        _nonWFLProductSalesArrangementAccess = nonWFLProductSalesArrangementAccess;
        _salesArrangementAuthorization = salesArrangementAuthorization;
    }

    public async Task Handle(SignDocumentManuallyRequest request, CancellationToken cancellationToken)
    {
        // validace opravneni
        await _salesArrangementAuthorization.ValidateDocumentSigningMngBySaType237And246BySAId(request.SalesArrangementId, cancellationToken);

        var documentOnSas = await _documentOnSaClient.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);

        if (!documentOnSas.DocumentsOnSAToSign.Any(d => d.DocumentOnSAId == request.DocumentOnSAId))
        {
            throw new NobyValidationException($"DocumentOnSa {request.DocumentOnSAId} not exist for SalesArrangement {request.SalesArrangementId}");
        }

        var documentOnSa = documentOnSas.DocumentsOnSAToSign.Single(r => r.DocumentOnSAId == request.DocumentOnSAId);

        if (documentOnSa.Source != DomainServices.DocumentOnSAService.Contracts.Source.Workflow)
            await _nonWFLProductSalesArrangementAccess.CheckNonWFLProductSalesArrangementAccess(documentOnSa.SalesArrangementId, cancellationToken);

        await _documentOnSaClient.SignDocument(request.DocumentOnSAId, SignatureTypes.Paper.ToByte(), cancellationToken);
    }
}
