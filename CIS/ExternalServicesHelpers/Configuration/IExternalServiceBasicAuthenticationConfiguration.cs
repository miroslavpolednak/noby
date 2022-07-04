namespace CIS.ExternalServicesHelpers.Configuration;

public interface IExternalServiceBasicAuthenticationConfiguration
    : IExternalServiceConfiguration
{
    string? Username { get; set; }

    string? Password { get; set; }
}
