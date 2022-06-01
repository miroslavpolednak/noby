namespace DomainServices.SalesArrangementService.Api.Dto;

internal record LinkCustomerOnSAToHouseholdMediatrRequest(Contracts.LinkCustomerOnSAToHouseholdRequest Request)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
}
