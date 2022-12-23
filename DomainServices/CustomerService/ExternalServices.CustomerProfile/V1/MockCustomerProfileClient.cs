namespace DomainServices.CustomerService.ExternalServices.CustomerProfile.V1;

internal sealed class MockCustomerProfileClient 
    : ICustomerProfileClient
{
    public Task<bool> ValidateProfile(long customerId, string profileCode, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.FromResult(true);
    }
}