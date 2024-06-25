using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.PatchDeveloperOnRealEstateValuation;

internal sealed class PatchDeveloperOnRealEstateValuationHandler(IRealEstateValuationServiceClient _realEstateValuationService)
    : IRequestHandler<RealEstateValuationPatchDeveloperOnRealEstateValuationRequest>
{
    public async Task Handle(RealEstateValuationPatchDeveloperOnRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var instance = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        // podvrhnute caseId
        if (instance.CaseId != request.CaseId
            || !instance.DeveloperAllowed)
        {
            throw new NobyValidationException(90032, "Case or developer check failed");
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
            throw new NobyValidationException(90032, "ValuationState check failed");
        }

        await _realEstateValuationService.PatchDeveloperOnRealEstateValuation(request.RealEstateValuationId, valuationStateId.Value, request.DeveloperApplied, cancellationToken);
    }
}
