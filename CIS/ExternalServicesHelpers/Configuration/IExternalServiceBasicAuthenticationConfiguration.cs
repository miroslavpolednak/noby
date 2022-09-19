using System.Net.Http.Headers;

namespace CIS.ExternalServicesHelpers.Configuration;

public interface IExternalServiceBasicAuthenticationConfiguration
    : IExternalServiceConfiguration
{
    AuthenticationHeaderValue HttpBasicAuthenticationHeader { get; }

    string? Username { get; set; }

    string? Password { get; set; }
}
