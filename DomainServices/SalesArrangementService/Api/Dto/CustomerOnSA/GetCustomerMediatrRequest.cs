namespace DomainServices.SalesArrangementService.Api.Dto;

internal record class GetCustomerMediatrRequest(int CustomerOnSAId)
    : IRequest<Contracts.CustomerOnSA>
{
}