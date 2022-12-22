using CIS.ExternalServicesHelpers.Configuration;

namespace DomainServices.CustomerService.Api.Configuration;

public class IdentifiedSubjectBrConfiguration : ExternalServiceBasicAuthenticationConfiguration
{
    public Clients.IdentifiedSubjectBr.Version Version { get; set; }

    public override string GetVersion() => Version.ToString();
}