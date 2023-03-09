using NOBY.Infrastructure.Security;

namespace NOBY.Infrastructure.ErrorHandling;

/// <summary>
/// Chyba autentizace - anonymni uzivatel
/// </summary>
public sealed class ApiAuthenticationProblemDetail
{
    public ApiAuthenticationProblemDetail() { }

    public ApiAuthenticationProblemDetail(string? redirectUri, string? authenticationScheme) 
    {
        RedirectUri = redirectUri ?? AuthenticationConstants.DefaultSignInUrl;
        AuthenticationScheme = authenticationScheme ?? "Unknown";
    }

    /// <summary>
    /// URI CAASu kam ma byt presmerovan browser klienta pro provedeni autentizace.
    /// </summary>
    public string RedirectUri { get; set; } = string.Empty;

    /// <summary>
    /// Typ autentizaci pouzity pro aktualni instanci FE API
    /// </summary>
    public string AuthenticationScheme { get; set; } = string.Empty;
}
