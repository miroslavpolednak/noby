namespace NOBY.Services.Customer;

public record CustomerInfoWithChangedData : CustomerInfo
{
    public required DomainServices.CustomerService.Contracts.Customer CustomerWithChangedData { get; init; }
}