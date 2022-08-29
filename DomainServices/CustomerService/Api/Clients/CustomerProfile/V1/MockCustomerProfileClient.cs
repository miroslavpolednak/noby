using CIS.Core.Results;

namespace DomainServices.CustomerService.Api.Clients.CustomerProfile.V1;

public class MockCustomerProfileClient : ICustomerProfileClient
{
    public Task<IServiceCallResult> ValidateProfile(long customerId, string profileCode, string traceId, CancellationToken cancellationToken)
    {
        return Task.FromResult<IServiceCallResult>(new SuccessfulServiceCallResult<bool>(true));
    }
}