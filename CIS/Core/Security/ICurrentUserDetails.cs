namespace CIS.Core.Security;

/// <summary>
/// Další informace o uživateli.
/// </summary>
/// <remarks>Tyto informace nejsou v <see cref="ICurrentUser"/>, protože se mohou systém od systému lišit, ale ICurrentUser je stejný pro všechny a nelze ho upravovat per systém.</remarks>
public interface ICurrentUserDetails
{
    /// <summary>
    /// Celé jméno uživatele
    /// </summary>
    /// <remarks>Filip Tůma</remarks>
    string DisplayName { get; }
}