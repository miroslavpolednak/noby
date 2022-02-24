namespace DomainServices.SalesArrangementService.Api.Dto;

internal record class GetCustomerListMediatrRequest(int SalesArrangementId)
    : IRequest<Contracts.GetCustomerListResponse>
{
    
}