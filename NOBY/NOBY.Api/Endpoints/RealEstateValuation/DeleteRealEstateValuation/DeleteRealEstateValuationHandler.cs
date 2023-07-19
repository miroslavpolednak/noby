using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.DeleteRealEstateValuation;

internal sealed class DeleteRealEstateValuationHandler
    : IRequestHandler<DeleteRealEstateValuationRequest>
{
    public async Task Handle(DeleteRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var instance = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        // podvrhnute caseId
        if (instance.RealEstateValuationGeneralDetails.CaseId != request.CaseId)
        {
            throw new CisAuthorizationException();
        }

        // spatny stav REV
        if (instance.RealEstateValuationGeneralDetails.ValuationStateId != 7)
        {
            throw new CisAuthorizationException();
        }

        await _realEstateValuationService.DeleteRealEstateValuation(request.CaseId, request.RealEstateValuationId, cancellationToken);
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public DeleteRealEstateValuationHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
