namespace CIS.Infrastructure.Security.Configuration;

/// <summary>
/// Sekce v appsettings.json pro nastaveni autentizace
/// </summary>
public class CisSecurityConfiguration
{
    /// <summary>
    /// Nastavení autentizace doménových služeb
    /// </summary>
    public CisServiceAuthenticationConfiguration? ServiceAuthentication { get; set; }
}
