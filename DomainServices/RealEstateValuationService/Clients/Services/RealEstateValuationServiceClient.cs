using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Clients.Services;

internal sealed class RealEstateValuationServiceClient
    : IRealEstateValuationServiceClient
{
    public async Task<int> CreateRealEstateValuation(CreateRealEstateValuationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateRealEstateValuationAsync(request, cancellationToken: cancellationToken);
        return result.RealEstateValuationId;
    }

    public async Task DeleteRealEstateValuation(long caseId, int realEstateValuationId, CancellationToken cancellationToken = default)
    {
        await _service.DeleteRealEstateValuationAsync(new DeleteRealEstateValuationRequest
        {
            CaseId = caseId,
            RealEstateValuationId = realEstateValuationId
        }, cancellationToken: cancellationToken);
    }

    public async Task<List<RealEstateValuationListItem>> GetRealEstateValuationList(long caseId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetRealEstateValuationListAsync(new GetRealEstateValuationListRequest
        {
            CaseId = caseId
        }, cancellationToken: cancellationToken);
        return result.RealEstateValuationList.ToList();
    }

    public async Task PatchDeveloperOnRealEstateValuation(int realEstateValuationId, int valuationStateId, bool developerApplied, CancellationToken cancellationToken = default)
    {
        await _service.PatchDeveloperOnRealEstateValuationAsync(new PatchDeveloperOnRealEstateValuationRequest
        {
            RealEstateValuationId = realEstateValuationId,
            ValuationStateId = valuationStateId,
            DeveloperApplied = developerApplied
        }, cancellationToken: cancellationToken);
    }

    private readonly Contracts.v1.RealEstateValuationService.RealEstateValuationServiceClient _service;
    public RealEstateValuationServiceClient(Contracts.v1.RealEstateValuationService.RealEstateValuationServiceClient service)
        => _service = service;
}
