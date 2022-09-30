namespace DomainServices.HouseholdService.Api.Handlers.Household.LinkCustomerOnSAToHousehold;

internal record LinkCustomerOnSAToHouseholdMediatrRequest(Contracts.LinkCustomerOnSAToHouseholdRequest Request)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
}
