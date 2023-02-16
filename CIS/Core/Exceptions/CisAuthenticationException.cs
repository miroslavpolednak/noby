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
    /// <remarks>Je použité v případě, že se jedná o autentizaci frontendu, kdy chceme FE vrátit informaci o tom, kam má uživatele přesměrovat.</remarks>
    public string? ProviderLoginUrl { get; init; }

    public CisAuthenticationException(string? providerLoginUrl = null , string? message = null)
        : base(message)
    {
        ProviderLoginUrl = providerLoginUrl;
    }
}
