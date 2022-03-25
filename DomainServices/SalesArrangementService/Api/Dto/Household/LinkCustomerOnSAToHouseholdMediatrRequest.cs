namespace DomainServices.SalesArrangementService.Api.Dto;

internal record LinkCustomerOnSAToHouseholdMediatrRequest(Contracts.LinkCustomerOnSAToHouseholdRequest Request)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
