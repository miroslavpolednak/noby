namespace DomainServices.DocumentArchiveService.Api.Configuration;

internal static class AppConfigurationExtensions
{
    /// <summary>
    /// Vrati ItChannel pro C4M, ktery je platny pro aktualniho service usera
    /// </summary>
    public static string GetLoginFromServiceUser(this AppConfiguration configuration, string serviceUser)
    {
        if (configuration.ServiceUser2LoginBinding is null || configuration.ServiceUser2LoginBinding.Count == 0)
            throw new CisConfigurationException(0, "ServiceUser2LoginBinding configuration is not set");

        if (configuration.ServiceUser2LoginBinding.TryGetValue(serviceUser, out string? value))
            return value;
        else if (configuration.ServiceUser2LoginBinding.TryGetValue("_default", out string? valueDefault))
            return valueDefault;
        else
            throw new CisConfigurationException(0, $"ServiceUser '{serviceUser}' not found in ServiceUser2LoginBinding configuration and no _default has been set");
    }
}
