namespace CIS.Infrastructure.ExternalServicesHelpers.Configuration;

/// <summary>
/// Výchozí implementace IExternalServiceBasicAuthenticationConfiguration
/// </summary>
public class ExternalServiceBasicAuthenticationConfiguration<TClient>
    : ExternalServiceConfiguration<TClient>, IExternalServiceBasicAuthenticationConfiguration
    where TClient : class

{
    /// <summary>
    /// Username
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Heslo
    /// </summary>
    public string? Password { get; set; }
}
