namespace NOBY.Infrastructure.Security;

public static class AuthenticationConstants
{
    /// <summary>
    /// Název cookie do které bude uložena identita uživatele
    /// </summary>
    public const string CookieName = "nobyauth";

    /// <summary>
    /// Mock authentication scheme name. Also used in appsettings.json to specify Mock Authentication provider to be used.
    /// </summary>
    public const string MockAuthScheme = "FomsMockAuthentication";

    /// <summary>
    /// Simple login authentication scheme name.
    /// </summary>
    public const string SimpleLoginAuthScheme = "SimpleLoginAuthentication";

    /// <summary>
    /// Autentizace vůči CAAS
    /// </summary>
    public const string CaasAuthScheme = "CaasAuthentication";

    /// <summary>
    /// Parametr v QueryStringu, ktery pri presmerovani na autentizacni autoritu rika, kde je ulozen redirectUri.
    /// </summary>
    public const string RedirectUriQueryParameter = "redirect";

    public const string DefaultAuthenticationUrlPrefix = "auth";
    public const string DefaultAuthenticationUrlSegment = "/" + DefaultAuthenticationUrlPrefix;
    public const string DefaultSignInEndpoint = "/signin";
    public const string DefaultSignOutEndpoint = "/signout";
    public const string DefaultSignInUrl = DefaultAuthenticationUrlSegment + DefaultSignInEndpoint;
    public const string DefaultSignOutUrl = DefaultAuthenticationUrlSegment + DefaultSignOutEndpoint;
}
