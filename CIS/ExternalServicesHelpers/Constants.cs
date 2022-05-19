namespace CIS.ExternalServicesHelpers;

public static class Constants
{
    /// <summary>
    /// Name of section in appsettings.json which holds configuration settings for external services.
    /// </summary>
    public const string ExternalServicesConfigurationSectionName = "ExternalServices";

    /// <summary>
    /// Prefix for service's Key in ServiceDiscovery database.
    /// </summary>
    public const string ExternalServicesServiceDiscoveryKeyPrefix = "ES:";
}
