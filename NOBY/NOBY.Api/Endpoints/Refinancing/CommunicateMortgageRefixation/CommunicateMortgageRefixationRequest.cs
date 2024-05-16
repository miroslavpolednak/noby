using NOBY.Dto.Refinancing;

namespace NOBY.Api.Endpoints.Refinancing.CommunicateMortgageRefixation;

public class CommunicateMortgageRefixationRequest : IRequest<RefinancingLinkResult>
{
    internal long CaseId { get; init; }
}