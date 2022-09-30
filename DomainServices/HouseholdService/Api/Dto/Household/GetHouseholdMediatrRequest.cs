using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Dto;

internal record GetHouseholdMediatrRequest(int HouseholdId)
    : IRequest<Household>
{
}