namespace NOBY.Infrastructure.Configuration;

public sealed class AppConfigurationSecurity
{
    /// <summary>
    /// Domena pro kterou bude platna autentizacni cookie
    /// </summary>
    public string? AuthenticationCookieDomain { get; set; }

    /// <summary>
    /// What kind of authentication provider to use - referes to DefaultScheme from .AddAuthentication().
    /// Possible values: FomsMockAuthentication
    /// </summary>
    public string AuthenticationScheme { get; set; } = string.Empty!;

    /// <summary>
    /// Pocet minut po jejich uplynuti bude uzivatel odhlasen, pokud v te dobe neprovedl aktivni request.
    /// </summary>
    public int? SessionInactivityTimeout { get; set; }

    /// <summary>
    /// oAuth authority
    /// </summary>
    public string? Authority { get; set; }

    /// <summary>
    /// oAuth ClientId
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// oAuth Client secret
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// Pokud je nastaveno na True, použije pro oAuth2 backchannel systémovou proxy
    /// </summary>
    public bool UseSystemProxy { get; set; }

    /// <summary>
    /// Vychozi URL na ktere bude uzivatel presmerovan po uspesnem prihlaseni, pokud neni mozne zjistit jeho puvodni URI.
    /// </summary>
    public string DefaultRedirectPathAfterSignIn { get; set; } = "/#";
}
