namespace NOBY.Api.Endpoints.Refinancing.GetInterestRatesValidFrom;

internal sealed record GetInterestRatesValidFromRequest(long CaseId)
    : IRequest<RefinancingGetInterestRatesValidFromResponse>
{
}
