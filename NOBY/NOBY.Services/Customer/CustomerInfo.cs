using DomainServices.HouseholdService.Contracts;

namespace NOBY.Services.Customer;

public record CustomerInfo
{
    public required CustomerOnSA CustomerOnSA { get; init; }

    public required DomainServices.CustomerService.Contracts.Customer CustomerDetail { get; init; }
}