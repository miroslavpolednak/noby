namespace NOBY.Api.Endpoints.Cases.GetCustomers;

internal sealed record GetCustomersRequest(long CaseId)
    : IRequest<List<GetCustomersResponseCustomer>>
{
}
