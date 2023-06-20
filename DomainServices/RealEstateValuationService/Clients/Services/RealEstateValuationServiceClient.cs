using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Clients.Services;

internal sealed class RealEstateValuationServiceClient
    : IRealEstateValuationServiceClient
{
    public async Task<int> CreateRealEstateValuation(CreateRealEstateValuationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateRealEstateValuationAsync(request, cancellationToken: cancellationToken);
        return result.NobyOrderId;
    }

    public async Task DeleteRealEstateValuation(long caseId, int nobyOrderId, CancellationToken cancellationToken = default)
    {
        await _service.DeleteRealEstateValuationAsync(new DeleteRealEstateValuationRequest
        {
            CaseId = caseId,
            NobyOrderId = nobyOrderId
        }, cancellationToken: cancellationToken);
    }

    public async Task<GetRealEstateValuationListResponse> GetRealEstateValuationList(long caseId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetRealEstateValuationListAsync(new GetRealEstateValuationListRequest
        {
            CaseId = caseId
        }, cancellationToken: cancellationToken);
        return result;
    }

    private readonly Contracts.v1.RealEstateValuationService.RealEstateValuationServiceClient _service;
    public RealEstateValuationServiceClient(Contracts.v1.RealEstateValuationService.RealEstateValuationServiceClient service)
        => _service = service;
}
