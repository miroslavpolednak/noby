using CIS.Core.Results;

namespace DomainServices.CustomerService.Api.Clients.CustomerProfile.V1;

public interface ICustomerProfileClient
{
    Task<IServiceCallResult> ValidateProfile(long customerId, string profileCode, string traceId, CancellationToken cancellationToken);
}