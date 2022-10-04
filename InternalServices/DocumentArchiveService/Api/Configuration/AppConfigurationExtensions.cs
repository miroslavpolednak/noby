namespace CIS.InternalServices.DocumentArchiveService.Api;

internal static class AppConfigurationExtensions
{
    /// <summary>
    /// Vrati ItChannel pro C4M, ktery je platny pro aktualniho service usera
    /// </summary>
    public static string GetLoginFromServiceUser(this AppConfiguration configuration, string serviceUser)
    {
        if (configuration.ServiceUser2LoginBinding is null || !configuration.ServiceUser2LoginBinding.Any())
            throw new CisConfigurationException(0, "ServiceUser2LoginBinding configuration is not set");

        if (configuration.ServiceUser2LoginBinding.ContainsKey(serviceUser))
            return configuration.ServiceUser2LoginBinding[serviceUser];
        else if (configuration.ServiceUser2LoginBinding.ContainsKey("_default"))
            return configuration.ServiceUser2LoginBinding["_default"];
        else
            throw new CisConfigurationException(0, $"ServiceUser '{serviceUser}' not found in ServiceUser2LoginBinding configuration and no _default has been set");
    }
}
