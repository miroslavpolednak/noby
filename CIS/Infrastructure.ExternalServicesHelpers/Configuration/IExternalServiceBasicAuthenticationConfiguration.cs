namespace CIS.Infrastructure.ExternalServicesHelpers.Configuration;

/// <summary>
/// Konfigurace pro podporu HTTP Basic Authentication.
/// </summary>
public interface IExternalServiceBasicAuthenticationConfiguration
{
    /// <summary>
    /// Username
    /// </summary>
    string? Username { get; set; }

    /// <summary>
    /// Heslo
    /// </summary>
    string? Password { get; set; }
}
