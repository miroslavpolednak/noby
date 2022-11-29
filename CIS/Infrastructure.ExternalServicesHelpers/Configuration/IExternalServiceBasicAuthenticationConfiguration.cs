namespace CIS.Infrastructure.ExternalServicesHelpers.Configuration;

/// <summary>
/// Konfigurace pro podporu HTTP Basic Authentication.
/// </summary>
public interface IExternalServiceBasicAuthenticationConfiguration
{
    string? Username { get; set; }

    string? Password { get; set; }
}
