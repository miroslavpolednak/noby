namespace CIS.Core.Exceptions;

/// <summary>
/// Chyba, která označuje problém s validací koncového uživatele.
/// </summary>
public sealed class CisAuthenticationException
    : System.Security.Authentication.AuthenticationException
{
    /// <summary>
    /// Adresa providera autentizace, na kterou má být uživatel přesměrován pro přihlášení.
    /// </summary>
    public string ProviderLoginUrl { get; init; }

    public CisAuthenticationException(string providerLoginUrl, string? message = null)
        : base(message)
    {
        ProviderLoginUrl = providerLoginUrl;
    }
}
