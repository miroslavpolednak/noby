namespace NOBY.Api.Endpoints.Refinancing.CommunicateMortgageRefixation;

public class CommunicateMortgageRefixationRequest : IRequest<CommunicateMortgageRefixationResponse>
{
    internal long CaseId { get; init; }
}