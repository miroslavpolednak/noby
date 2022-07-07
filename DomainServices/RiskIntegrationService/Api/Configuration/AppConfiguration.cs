namespace DomainServices.RiskIntegrationService.Api;

internal sealed class AppConfiguration
{
    /// <summary>
    /// Nastaveni mapovani technickych uzivatelu sluzby vs. ItChannel C4M
    /// [service_user, C4M ItChannel]
    /// </summary>
    public Dictionary<string, string>? ServiceUser2ItChannelBinding { get; set; }
}
