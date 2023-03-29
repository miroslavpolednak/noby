namespace CIS.Core.Exceptions;

/// <summary>
/// Pokud nastavení konfigurace v appsettings.json pro danou konfigurační sekci nelze zvalidovat - tj. chybí nastavit některá z required props atd.
/// </summary>
public sealed class CisConfigurationException
    : BaseCisException
{
    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public CisConfigurationException(int exceptionCode, string? message)
        : base(exceptionCode, message)
    { }
}