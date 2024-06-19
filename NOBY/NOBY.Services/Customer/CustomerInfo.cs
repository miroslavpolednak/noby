using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;

namespace NOBY.Services.Customer;

public record CustomerInfo
{
    public required CustomerOnSA CustomerOnSA { get; init; }

    public required CustomerDetailResponse CustomerDetail { get; init; }
}