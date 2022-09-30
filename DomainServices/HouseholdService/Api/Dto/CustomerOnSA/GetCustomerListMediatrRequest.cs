namespace DomainServices.HouseholdService.Api.Dto;

internal record class GetCustomerListMediatrRequest(int SalesArrangementId)
    : IRequest<Contracts.GetCustomerListResponse>
{
    
}