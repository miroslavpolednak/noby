using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Handlers.Household.CreateHousehold;

internal record CreateHouseholdMediatrRequest(CreateHouseholdRequest Request)
    : IRequest<CreateHouseholdResponse>, CIS.Core.Validation.IValidatableRequest
{
}