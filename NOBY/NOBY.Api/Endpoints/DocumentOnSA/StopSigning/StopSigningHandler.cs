using DomainServices.DocumentOnSAService.Clients;
using FastEnumUtility;
using NOBY.Services.PermissionAccess;
using NOBY.Services.SigningHelper;

namespace NOBY.Api.Endpoints.DocumentOnSA.StopSigning;

public class StopSigningHandler : IRequestHandler<StopSigningRequest>
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly INonWFLProductSalesArrangementAccess _nonWFLProductSalesArrangementAccess;
    private readonly ISigningHelperService _signingHelperService;

    public StopSigningHandler(
        IDocumentOnSAServiceClient documentOnSAService,
        INonWFLProductSalesArrangementAccess nonWFLProductSalesArrangementAccess,
        ISigningHelperService signingHelperService)
    {
        _documentOnSAService = documentOnSAService;
        _nonWFLProductSalesArrangementAccess = nonWFLProductSalesArrangementAccess;
        _signingHelperService = signingHelperService;
    }

    public async Task Handle(StopSigningRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _signingHelperService.GetDocumentOnSa(new()
        {
            DocumentOnSAId = request.DocumentOnSAId,
            SalesArrangementId = request.SalesArrangementId,
        }, cancellationToken);

        if (documentOnSa.Source != DomainServices.DocumentOnSAService.Contracts.Source.Workflow)
            await _nonWFLProductSalesArrangementAccess.CheckNonWFLProductSalesArrangementAccess(documentOnSa.SalesArrangementId, cancellationToken);

        if (documentOnSa.DocumentTypeId == DocumentTypes.DANRESID.ToByte() && documentOnSa.IsSigned) // 13
            throw new NobyValidationException(90036);

        await _signingHelperService.StopSinningAccordingState(new()
        {
            DocumentOnSAId = request.DocumentOnSAId,
            SalesArrangementId = request.SalesArrangementId,
            SignatureTypeId = documentOnSa.SignatureTypeId,
        }, cancellationToken);
    }

   
}
