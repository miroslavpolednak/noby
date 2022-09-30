namespace DomainServices.HouseholdService.Api.Dto;

internal record class GetCustomerMediatrRequest(int CustomerOnSAId)
    : IRequest<Contracts.CustomerOnSA>
{
}