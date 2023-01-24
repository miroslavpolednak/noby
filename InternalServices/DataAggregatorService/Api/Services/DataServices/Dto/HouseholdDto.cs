using DomainServices.HouseholdService.Contracts;
using Household = DomainServices.HouseholdService.Contracts.Household;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.Dto;

internal class HouseholdDto
{
    public required Household Household { get; init; }

    public CustomerOnSA? CustomerOnSa1 { get; init; }

    public CustomerOnSA? CustomerOnSa2 { get; init; }
}