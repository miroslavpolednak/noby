using CIS.Core.Validation;

namespace NOBY.Api.Endpoints.SalesArrangement.GetCustomers;

internal sealed record GetCustomersRequest(int SalesArrangementId)
    : IRequest<List<Dto.CustomerListItem>>
{
}