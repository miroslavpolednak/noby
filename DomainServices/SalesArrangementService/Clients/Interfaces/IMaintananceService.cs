using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Clients;

public interface IMaintananceService
{
    Task CancelNotFinishedExtraPayments(CancellationToken cancellationToken = default);
    
    Task<long[]> GetCancelCaseJobIds(CancellationToken cancellationToken = default);

    Task<int[]> GetCancelServiceSalesArrangementsIds(CancellationToken cancellationToken = default);

    Task<GetOfferGuaranteeDateToCheckResponse.Types.GetOfferGuaranteeDateToCheckItem[]> GetOfferGuaranteeDateToCheck(CancellationToken cancellationToken = default);
}
