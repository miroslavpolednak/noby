using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Clients;

internal sealed class MaintananceService(Contracts.MaintananceService.MaintananceServiceClient _service)
    : IMaintananceService
{
    public async Task<long[]> GetCancelCaseJobIds(CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCancelCaseJobIdsAsync(new(), cancellationToken: cancellationToken);
        return result.CaseId?.ToArray() ?? Array.Empty<long>();
    }

    public async Task<int[]> GetCancelServiceSalesArrangementsIds(CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCancelServiceSalesArrangementsIdsAsync(new(), cancellationToken: cancellationToken);
        return result.SalesArrangementId?.ToArray() ?? Array.Empty<int>();
    }

    public async Task<GetOfferGuaranteeDateToCheckResponse.Types.GetOfferGuaranteeDateToCheckItem[]> GetOfferGuaranteeDateToCheck(CancellationToken cancellationToken = default)
    {
        var result = await _service.GetOfferGuaranteeDateToCheckAsync(new(), cancellationToken: cancellationToken);
        return result.Items?.ToArray() ?? Array.Empty<GetOfferGuaranteeDateToCheckResponse.Types.GetOfferGuaranteeDateToCheckItem>();
    }
}
