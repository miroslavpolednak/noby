using CIS.ExternalServicesHelpers.Configuration;
using DomainServices.CustomerService.Api.Clients;

namespace DomainServices.CustomerService.Api.Configuration;

public class CustomerManagementConfiguration : ExternalServiceBaseConfiguration
{
    public CMVersion Version { get; set; } = CMVersion.Unknown;
}