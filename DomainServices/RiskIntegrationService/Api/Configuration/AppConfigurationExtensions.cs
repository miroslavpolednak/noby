namespace DomainServices.RiskIntegrationService.Api;

internal static class AppConfigurationExtensions
{
    /// <summary>
    /// Vrati ItChannel pro C4M, ktery je platny pro aktualniho service usera
    /// </summary>
    public static string GetItChannelFromServiceUser(this AppConfiguration configuration, string? serviceUser)
    {
        if (configuration.ServiceUser2ItChannelBinding is null || !configuration.ServiceUser2ItChannelBinding.Any())
            throw ErrorCodeMapper.CreateConfigurationException(ErrorCodeMapper.ServiceUserIsNull);

        if (configuration.ServiceUser2ItChannelBinding.TryGetValue(serviceUser ?? "_default", out string? v))
            return v ?? "";
        else
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.ServiceUserNotFound, serviceUser);
    }
}
