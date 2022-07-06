namespace CIS.Core.Security;

/// <summary>
/// Helper pro ziskani fyzickeho uzivatele, ktery aplikaci/sluzbu vola
/// </summary>
public interface ICurrentUserAccessor
{
    /// <summary>
    /// Pokud je false, uzivatel neni autentikovan - User = null
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Zakladni data o uzivateli - Id, login?
    /// </summary>
    ICurrentUser? User { get; }

    /// <summary>
    /// Kompletni profil uzivatele - neni implicitne naplnen. Pro jeho naplneni je potreba zavolat FetchDetails().
    /// </summary>
    ICurrentUserDetails? UserDetails { get; }

    /// <summary>
    /// Pokud se tak uz nestalo, naplni profil uzivatele daty z UserService
    /// </summary>
    Task<ICurrentUserDetails> EnsureDetails(CancellationToken cancellationToken);

    Task<TDetails> EnsureDetails<TDetails>(CancellationToken cancellationToken) 
        where TDetails : ICurrentUserDetails;
}