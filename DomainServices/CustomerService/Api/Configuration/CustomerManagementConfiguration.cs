using CIS.ExternalServicesHelpers.Configuration;
using DomainServices.CustomerService.Api.Clients;

namespace DomainServices.CustomerService.Api.Configuration;

public class CustomerManagementConfiguration : ExternalServiceBasicAuthenticationConfiguration
{
    public override string GetVersion() => this.Version.ToString();

    public CMVersion Version { get; set; } = CMVersion.Unknown;
}