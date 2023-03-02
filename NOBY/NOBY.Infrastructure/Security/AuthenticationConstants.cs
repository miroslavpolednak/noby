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
    /// Type claimu, který obsahuje v33id přihlášeného uživatele
    /// </summary>
    public const string ClaimNameV33id = "v33id";

    /// <summary>
    /// Type claimu, který obsahuje login (CAAS login) přihlášeného uživatele
    /// </summary>
    public const string ClaimNameLogin = "login";
}
