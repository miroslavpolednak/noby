using CIS.Core.Validation;

namespace NOBY.Api.Endpoints.SalesArrangement.GetCustomers;

internal record GetCustomersRequest(int SalesArrangementId)
    : IRequest<List<Dto.CustomerListItem>>
{
}