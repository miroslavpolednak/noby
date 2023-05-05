namespace CIS.Core.Configuration;

/// <summary>
/// Konfigurace služby v prostředí NOBY. Binduje se z elementu *CisEnvironmentConfiguration* v appsettings.json.
/// </summary>
public interface ICisEnvironmentConfiguration
{
    /// <summary>
    /// Název služby v systému NOBY
    /// </summary>
    /// <remarks>[typ_sluzby]:[nazev_sluzby] - DS (Doménová služba), CIS (Infrastrukturní služba)</remarks>
    /// <example>DS:CustomerService</example>
    string? DefaultApplicationKey { get; }

    /// <summary>
    /// Název aplikačního prostředí
    /// </summary>
    /// <remarks>Povolené hodnoty: DEV, FAT, UAT, SIT, PROD</remarks>
    /// <example>DEV</example>
    string? EnvironmentName { get; }

    /// <summary>
    /// Adresa Service discovery služby pro dané prostředí
    /// </summary>
    /// <remarks>Pokud není zadáno, nebude fungovat automatické dohledávání adres služeb v Clients projektech.</remarks>
    /// <example>https://177.0.0.55:30000</example>
    string? ServiceDiscoveryUrl { get; }

    /// <summary>
    /// Pokud je nastaveno na True, nebude se v infrastruktuře služeb automaticky dotahovat URL návazných komponent z ServiceDiscovery.
    /// </summary>
    /// <remarks>Pravděpodobně se použije pouze pro infra testy.</remarks>
    bool DisableServiceDiscovery { get; }

    /// <summary>
    /// Login uživatele pod kterým se daná služba autentizuje do ostatních služeb v rámci NOBY
    /// </summary>
    /// <remarks>Je nutné zadat pokud má fungovat automatické dohledávání adres služeb v Clients projektech.</remarks>
    /// <example>user_a</example>
    string? InternalServicesLogin { get; }

    /// <summary>
    /// Heslo servisního uživatele
    /// </summary>
    /// <example>passw0rd</example>
    string? InternalServicePassword { get; }
}