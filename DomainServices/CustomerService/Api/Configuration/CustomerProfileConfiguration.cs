using CIS.ExternalServicesHelpers.Configuration;
using DomainServices.CustomerService.Api.Clients.CustomerProfile;

namespace DomainServices.CustomerService.Api.Configuration;

public class CustomerProfileConfiguration : ExternalServiceBaseConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;
}