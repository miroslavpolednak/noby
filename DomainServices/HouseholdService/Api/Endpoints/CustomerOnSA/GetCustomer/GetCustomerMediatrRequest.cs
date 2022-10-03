namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetCustomer;

internal record class GetCustomerMediatrRequest(int CustomerOnSAId)
    : IRequest<Contracts.CustomerOnSA>
{
}