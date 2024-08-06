namespace NOBY.Api.Endpoints.Refinancing.GetAvailableFixedRatePeriods;

internal sealed record GetAvailableFixedRatePeriodsRequest(long CaseId)
    : IRequest<RefinancingGetAvailableFixedRatePeriodsResponse>
{
}
