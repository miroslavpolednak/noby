namespace NOBY.Api.Endpoints.Refinancing.CommunicateMortgageRefixation;

internal sealed record CommunicateMortgageRefixationRequest(long CaseId)
    : IRequest<RefinancingSharedOfferLinkResult>
{
}