namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;

internal sealed record GetMortgageRefixationRequest(long CaseId, long? ProcessId)
    : IRequest<RefinancingGetMortgageRefixationResponse>
{
}
