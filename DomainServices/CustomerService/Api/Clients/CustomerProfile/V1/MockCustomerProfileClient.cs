namespace DomainServices.CustomerService.Api.Clients.CustomerProfile.V1;

public class MockCustomerProfileClient : ICustomerProfileClient
{
    public Task<bool> ValidateProfile(long customerId, string profileCode, string traceId, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}