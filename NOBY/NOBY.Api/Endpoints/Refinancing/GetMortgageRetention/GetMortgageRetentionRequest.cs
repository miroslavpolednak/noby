namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRetention;

internal sealed record GetMortgageRetentionRequest(long CaseId, long ProcessId)
    : IRequest<RefinancingGetMortgageRetentionResponse>
{
}
