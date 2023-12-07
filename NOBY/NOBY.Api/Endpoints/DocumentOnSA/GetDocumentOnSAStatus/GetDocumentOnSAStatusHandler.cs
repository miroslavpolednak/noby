
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Extensions;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAStatus;

public class GetDocumentOnSAStatusHandler : IRequestHandler<GetDocumentOnSAStatusRequest, GetDocumentOnSAStatusResponse>
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public GetDocumentOnSAStatusHandler(
        IDocumentOnSAServiceClient documentOnSAService,
        ICodebookServiceClient codebookService,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _documentOnSAService = documentOnSAService;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
    }

    public async Task<GetDocumentOnSAStatusResponse> Handle(GetDocumentOnSAStatusRequest request, CancellationToken cancellationToken)
    {
        var docOnSaStatusData = await _documentOnSAService.GetDocumentOnSAStatus(request.SalesArrangementId, request.DocumentOnSAId, cancellationToken);

        var signatureStates = await _codebookService.SignatureStatesNoby(cancellationToken);
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        return new()
        {
            DocumentOnSAId = docOnSaStatusData.DocumentOnSAId,
            SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new()
            {
                IsValid = docOnSaStatusData.IsValid,
                DocumentOnSAId = docOnSaStatusData.DocumentOnSAId,
                IsSigned = docOnSaStatusData.IsSigned,
                Source = docOnSaStatusData.Source.MapToCisEnum(),
                SalesArrangementTypeId = salesArrangement.SalesArrangementTypeId,
                EArchivIdsLinked = docOnSaStatusData.EArchivIdsLinked
            },
          signatureStates)
        };
    }
}
