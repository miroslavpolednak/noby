namespace NOBY.Api.Endpoints.Offer.CommunicateMortgageRefixation;

public class CommunicateMortgageRefixationRequest : IRequest<CommunicateMortgageRefixationResponse>
{
    internal long CaseId { get; init; }
}