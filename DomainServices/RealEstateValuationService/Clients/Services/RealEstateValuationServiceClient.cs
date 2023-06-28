using DomainServices.RealEstateValuationService.Contracts;
using StackExchange.Redis;

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

    public async Task<RealEstateValuationDetail> GetRealEstateValuationDetail(int realEstateValuationId, CancellationToken cancellationToken = default)
    {
        return await _service.GetRealEstateValuationDetailAsync(new GetRealEstateValuationDetailRequest
        {
            RealEstateValuationId = realEstateValuationId
        }, cancellationToken: cancellationToken);
    }

    public async Task<RealEstateValuationDetail> GetRealEstateValuationDetailByOrderId(int orderId, CancellationToken cancellationToken = default)
    {
        return await _service.GetRealEstateValuationDetailByOrderIdAsync(new GetRealEstateValuationDetailByOrderIdRequest
        {
            OrderId = orderId
        }, cancellationToken: cancellationToken);
    }

    public async Task UpdateRealEstateValuationDetail(UpdateRealEstateValuationDetailRequest request, CancellationToken cancellationToken = default)
    {
        await _service.UpdateRealEstateValuationDetailAsync(request, cancellationToken: cancellationToken);
    }

    private readonly Contracts.v1.RealEstateValuationService.RealEstateValuationServiceClient _service;
    public RealEstateValuationServiceClient(Contracts.v1.RealEstateValuationService.RealEstateValuationServiceClient service)
        => _service = service;
}
