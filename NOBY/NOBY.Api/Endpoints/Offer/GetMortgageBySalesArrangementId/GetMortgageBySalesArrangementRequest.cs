using CIS.Core.Validation;

namespace NOBY.Api.Endpoints.Offer.GetMortgageBySalesArrangement;

internal sealed record GetMortgageBySalesArrangementRequest(int SalesArrangementId)
    : IRequest<Dto.GetMortgageResponse>
{
}