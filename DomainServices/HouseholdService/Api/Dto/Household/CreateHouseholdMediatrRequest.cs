using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Dto;

internal record CreateHouseholdMediatrRequest(CreateHouseholdRequest Request)
    : IRequest<CreateHouseholdResponse>, CIS.Core.Validation.IValidatableRequest
{
}