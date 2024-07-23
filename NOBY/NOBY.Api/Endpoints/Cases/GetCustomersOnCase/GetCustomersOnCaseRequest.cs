namespace NOBY.Api.Endpoints.Cases.GetCustomersOnCase;

internal sealed record GetCustomersOnCaseRequest(long CaseId)
    : IRequest<List<CasesGetCustomersResponseCustomer>>
{
}
