using CIS.ExternalServicesHelpers.Configuration;
using DomainServices.CustomerService.Api.Clients.CustomerManagement;

namespace DomainServices.CustomerService.Api.Configuration;

public class CustomerManagementConfiguration : ExternalServiceBaseConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;
}