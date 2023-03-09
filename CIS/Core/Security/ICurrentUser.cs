namespace CIS.Core.Security;

/// <summary>
/// Instance aktuálně přihlášeného fyzického uživatele
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// v33id
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Login uzivatele
    /// </summary>
    string? Login { get; }

    /// <summary>
    /// Jmeno a prijmeni uzivatele
    /// </summary>
    string? DisplayName { get; }
}
