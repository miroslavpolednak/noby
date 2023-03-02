namespace NOBY.Infrastructure.Configuration;

public sealed class AppConfigurationSecurity
{
    /// <summary>
    /// What kind of authentication provider to use - referes to DefaultScheme from .AddAuthentication().
    /// Possible values: FomsMockAuthentication
    /// </summary>
    public string AuthenticationScheme { get; set; } = "";

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
}
