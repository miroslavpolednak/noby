namespace DomainServices.RiskIntegrationService.Api;

internal static class AppConfigurationExtensions
{
    /// <summary>
    /// Vrati ItChannel pro C4M, ktery je platny pro aktualniho service usera
    /// </summary>
    public static string GetItChannelFromServiceUser(this AppConfiguration configuration, string serviceUser)
    {
        if (configuration.ServiceUser2ItChannelBinding is null || !configuration.ServiceUser2ItChannelBinding.Any())
            throw new CisConfigurationException(17002, "ServiceUser2ItChannelBinding configuration is not set");

        if (configuration.ServiceUser2ItChannelBinding.ContainsKey(serviceUser))
            return configuration.ServiceUser2ItChannelBinding[serviceUser];
        else if (configuration.ServiceUser2ItChannelBinding.ContainsKey("_default"))
            return configuration.ServiceUser2ItChannelBinding["_default"];
        else
            throw new CisConfigurationException(17003, $"ServiceUser '{serviceUser}' not found in ServiceUser2ItChannelBinding configuration and no _default has been set");
    }
}
