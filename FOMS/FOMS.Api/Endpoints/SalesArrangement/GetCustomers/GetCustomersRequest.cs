using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.SalesArrangement.GetCustomers;

internal record GetCustomersRequest(int SalesArrangementId)
    : IRequest<List<Dto.CustomerListItem>>, IValidatableRequest
{
    
}