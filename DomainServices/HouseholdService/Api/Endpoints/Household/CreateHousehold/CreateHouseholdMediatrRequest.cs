using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.CreateHousehold;

internal record CreateHouseholdMediatrRequest(CreateHouseholdRequest Request)
    : IRequest<CreateHouseholdResponse>, CIS.Core.Validation.IValidatableRequest
{
}