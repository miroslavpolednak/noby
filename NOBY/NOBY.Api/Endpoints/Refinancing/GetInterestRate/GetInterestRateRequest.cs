namespace NOBY.Api.Endpoints.Refinancing.GetInterestRate;

internal sealed record GetInterestRateRequest(long CaseId)
    : IRequest<RefinancingGetInterestRateResponse>
{
}
