using DomainServices.SalesArrangementService.Clients;
using FastEnumUtility;
using NOBY.Services.CheckNonWFLProductSalesArrangementAccess;
using NOBY.Services.SalesArrangementAuthorization;
using NOBY.Services.SigningHelper;

namespace NOBY.Api.Endpoints.DocumentOnSA.StopSigning;

public class StopSigningHandler : IRequestHandler<StopSigningRequest>
{
    private readonly INonWFLProductSalesArrangementAccessService _nonWFLProductSalesArrangementAccess;
    private readonly ISigningHelperService _signingHelperService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public StopSigningHandler(
        INonWFLProductSalesArrangementAccessService nonWFLProductSalesArrangementAccess,
        ISigningHelperService signingHelperService,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _nonWFLProductSalesArrangementAccess = nonWFLProductSalesArrangementAccess;
        _signingHelperService = signingHelperService;
        _salesArrangementService = salesArrangementService;
    }

    public async Task Handle(StopSigningRequest request, CancellationToken cancellationToken)
    {
        var saInstance = await _salesArrangementService.ValidateSalesArrangementId(request.SalesArrangementId, true, cancellationToken);
        
        // nesmi se jedna o refinancovani
        if (ISalesArrangementAuthorizationService.RefinancingSATypes.Contains(saInstance.SalesArrangementTypeId!.Value))
        {
            throw new NobyValidationException(90032);
        }

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
