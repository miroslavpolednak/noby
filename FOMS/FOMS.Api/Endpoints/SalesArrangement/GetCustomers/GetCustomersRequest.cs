using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.SalesArrangement.Dto;

internal record GetCustomersRequest(int SalesArrangementId)
    : IRequest<List<Dto.CustomerListItem>>, IValidatableRequest
{
    
}