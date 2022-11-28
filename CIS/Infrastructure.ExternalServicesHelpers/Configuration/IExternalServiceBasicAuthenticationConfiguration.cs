using System.Net.Http.Headers;

namespace CIS.Infrastructure.ExternalServicesHelpers.Configuration;

public interface IExternalServiceBasicAuthenticationConfiguration
{
    string? Username { get; set; }

    string? Password { get; set; }
}
