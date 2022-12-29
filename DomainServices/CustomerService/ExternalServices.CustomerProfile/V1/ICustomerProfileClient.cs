using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CustomerService.ExternalServices.CustomerProfile.V1;

public interface ICustomerProfileClient
    : IExternalServiceClient
{
    Task<bool> ValidateProfile(long customerId, string profileCode, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";
}