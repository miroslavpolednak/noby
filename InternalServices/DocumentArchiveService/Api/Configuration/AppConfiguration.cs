namespace CIS.InternalServices.DocumentArchiveService.Api;

internal sealed class AppConfiguration
{
    /// <summary>
    /// Nastaveni mapovani technickych uzivatelu sluzby vs. Login v generovanem ID
    /// [service_user, C4M ItChannel]
    /// </summary>
    public Dictionary<string, string>? ServiceUser2LoginBinding { get; set; }
}
