namespace CIS.Core.Security;

public interface ICurrentUser
{
    /// <summary>
    /// v33id
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Login uzivatele
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Cele jmeno uzivatele
    /// </summary>
    string? DisplayName { get; }
}
