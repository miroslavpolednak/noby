namespace CIS.DomainServicesSecurity.Configuration;

public class CisServiceAuthenticationConfiguration
{
    public enum LoginValidators
    {
        ActiveDirectory,
        StaticCollection
    }

    public LoginValidators Validator { get; set; } = LoginValidators.ActiveDirectory;

    /// <summary>
    /// Domena ve ktere je umisten autentizovany uzivatel. Napr. "vsskb\"
    /// Pozor, musi byt vcetne \ na konci
    /// </summary>
    public string? DomainUsernamePrefix { get; set; }

    /// <summary>
    /// Adresa domenoveho serveru
    /// </summary>
    public string? AdHost { get; set; }

    /// <summary>
    /// Port na domenovem serveru, vychozi je 389
    /// </summary>
    public int? AdPort { get; set; } = 389;
}
