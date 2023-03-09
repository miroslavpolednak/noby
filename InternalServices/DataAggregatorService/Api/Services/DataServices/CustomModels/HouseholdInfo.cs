using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;

internal class HouseholdInfo
{
    public required Household Household { get; init; }

    public CustomerOnSA? CustomerOnSa1 { get; init; }

    public CustomerOnSA? CustomerOnSa2 { get; init; }
}