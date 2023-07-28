using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.PatchDeveloperOnRealEstateValuation;

internal sealed class PatchDeveloperOnRealEstateValuationHandler
    : IRequestHandler<PatchDeveloperOnRealEstateValuationRequest>
{
    public async Task Handle(PatchDeveloperOnRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var instance = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        // podvrhnute caseId
        if (instance.CaseId != request.CaseId
            || !instance.DeveloperAllowed)
        {
            throw new CisAuthorizationException();
        }

        int? valuationStateId = null;

        if (instance.ValuationStateId == 7)
        {
            valuationStateId = request.DeveloperApplied ? 4 : 7;
        }
        else if (instance.ValuationStateId == 4 && instance.DeveloperApplied && !request.DeveloperApplied)
        {
            valuationStateId = 7;
        }
        else
        {
            throw new CisAuthorizationException();
        }

        await _realEstateValuationService.PatchDeveloperOnRealEstateValuation(request.RealEstateValuationId, valuationStateId.Value, request.DeveloperApplied, cancellationToken);
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public PatchDeveloperOnRealEstateValuationHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
