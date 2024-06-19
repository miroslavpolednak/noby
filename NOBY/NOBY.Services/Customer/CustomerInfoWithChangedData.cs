using DomainServices.CustomerService.Contracts;

namespace NOBY.Services.Customer;

public record CustomerInfoWithChangedData : CustomerInfo
{
    public required CustomerDetailResponse CustomerWithChangedData { get; init; }
}