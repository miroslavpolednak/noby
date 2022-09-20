using CIS.ExternalServicesHelpers.Configuration;

namespace DomainServices.CustomerService.Api.Configuration;

public class CustomerManagementConfiguration : ExternalServiceBasicAuthenticationConfiguration
{
    public Clients.CustomerManagement.Version CustomerManagementVersion { get; set; }

    public Clients.CustomerProfile.Version CustomerProfileVersion { get; set; }

    public Clients.IdentifiedSubjectBr.Version IdentifiedSubjectVersion { get; set; }

    public override string GetVersion() => throw new NotImplementedException();
}