namespace NOBY.Api.Endpoints.Offer.GetMortgageBySalesArrangement;

internal sealed record GetMortgageBySalesArrangementRequest(int SalesArrangementId)
    : IRequest<SharedDto.GetMortgageResponse>
{
}