namespace CIS.Infrastructure.Security.Configuration;

/// <summary>
/// Konfigurace autentizace doménové služby
/// </summary>
public sealed class CisServiceAuthenticationConfiguration
{
    public enum LoginValidators
    {
        ActiveDirectory,
        NativeActiveDirectory,
        StaticCollection
    }

    public LoginValidators Validator { get; set; } = LoginValidators.ActiveDirectory;

    /// <summary>
    /// Domena ve ktere je umisten autentizovany uzivatel. Napr. "vsskb.cz"
    /// </summary>
    public string? Domain { get; set; }

    /// <summary>
    /// Adresa domenoveho serveru
    /// </summary>
    public string? AdHost { get; set; }

    /// <summary>
    /// Port na domenovem serveru, vychozi je 389
    /// </summary>
    public int? AdPort { get; set; } = 389;

    /// <summary>
    /// True pokud se jedna o SSL connection
    /// </summary>
    public bool IsSsl { get; set; }

    /// <summary>
    /// Seznam uživatelů, kteří mohou přistupovat k dané službě
    /// </summary>
    public List<AllowedUserConfiguration>? AllowedUsers { get; set; }

    public sealed class AllowedUserConfiguration
    {
        /// <summary>
        /// Login technického uživatele
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Seznam rolí ve kterých je uživatel zařazen - nemusí mít žádné
        /// </summary>
        public List<string>? Roles { get; set; }
    }
}
