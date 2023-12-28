namespace NOBY.Api.Endpoints.SalesArrangement.GetCustomers;

internal sealed record GetCustomersRequest(int SalesArrangementId)
    : IRequest<List<SharedDto.CustomerListItem>>
{
}