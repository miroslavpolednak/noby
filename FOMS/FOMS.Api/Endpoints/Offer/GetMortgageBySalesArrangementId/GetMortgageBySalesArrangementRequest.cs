using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Offer.Dto;

internal record GetMortgageBySalesArrangementRequest(int SalesArrangementId)
    : IRequest<GetMortgageResponse>, IValidatableRequest
{
}