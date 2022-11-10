namespace NOBY.Api.Endpoints.Cases.GetCustomers;

internal record GetCustomersRequest(long CaseId)
    : IRequest<List<GetCustomersResponseCustomer>>
{
}
