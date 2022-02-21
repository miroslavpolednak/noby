using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Offer.GetMortgageBySalesArrangement;

internal record GetMortgageBySalesArrangementRequest(int SalesArrangementId)
    : IRequest<Dto.GetMortgageResponse>, IValidatableRequest
{
}