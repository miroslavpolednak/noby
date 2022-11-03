using CIS.Core.Validation;

namespace NOBY.Api.Endpoints.Offer.GetMortgageBySalesArrangement;

internal record GetMortgageBySalesArrangementRequest(int SalesArrangementId)
    : IRequest<Dto.GetMortgageResponse>
{
}