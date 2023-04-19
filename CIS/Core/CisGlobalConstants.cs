namespace CIS.Core;

/// <summary>
/// Globální konstanty aplikace.
/// </summary>
public static class CisGlobalConstants
{
    /// <summary>
    /// URL health check endpointu pro vsechny sluzby/aplikace.
    /// </summary>
    public const string CisHealthCheckEndpointUrl = "/health";

    /// <summary>
    /// Name of section in appsettings.json which holds configuration settings for external services.
    /// </summary>
    public const string ExternalServicesConfigurationSectionName = "ExternalServices";

    /// <summary>
    /// Prefix for external service's Key in ServiceDiscovery database.
    /// </summary>
    public const string ExternalServicesServiceDiscoveryKeyPrefix = "ES:";

    /// <summary>
    /// Name of section in appsettings.json which holds configuration settings for domain services.
    /// </summary>
    public const string DomainServicesClientsConfigurationSectionName = "DomainServicesClients";

    /// <summary>
    /// Name of section in appsettings.json which holds CIS environment configuration
    /// </summary>
    public const string EnvironmentConfigurationSectionName = "CisEnvironmentConfiguration";
}