using CIS.ExternalServicesHelpers.Configuration;

namespace DomainServices.CustomerService.Api.Configuration;

public class CustomerManagementConfiguration : ExternalServiceBasicAuthenticationConfiguration
{
    public Clients.CustomerProfile.Version CustomerProfileVersion { get; set; }

    public override string GetVersion() => throw new NotImplementedException();
}