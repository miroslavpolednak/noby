namespace CIS.Core.Security;

/// <summary>
/// Helper pro ziskani instance technickeho uzivatele, pod kterym je spusten request na interni sluzbu.
/// </summary>
public interface IServiceUserAccessor
{
    /// <summary>
    /// Pokud je false, uzivatel neni autentikovan - User = null
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Instance technickeho uzivatele
    /// </summary>
    IServiceUser? User { get; }
}