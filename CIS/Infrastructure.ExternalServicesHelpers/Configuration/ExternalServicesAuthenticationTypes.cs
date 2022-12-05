namespace CIS.Infrastructure.ExternalServicesHelpers.Configuration;

/// <summary>
/// Možné typy autentizace na službu třetí strany.
/// </summary>
public enum ExternalServicesAuthenticationTypes
{
    /// <summary>
    /// Bez autentizace
    /// </summary>
    None = 1,

    /// <summary>
    /// HTTP Basic Authentication
    /// </summary>
    Basic = 2
}
