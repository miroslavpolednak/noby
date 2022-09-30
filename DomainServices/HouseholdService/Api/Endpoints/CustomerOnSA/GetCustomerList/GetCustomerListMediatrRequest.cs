namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetCustomerList;

internal record class GetCustomerListMediatrRequest(int SalesArrangementId)
    : IRequest<Contracts.GetCustomerListResponse>
{

}