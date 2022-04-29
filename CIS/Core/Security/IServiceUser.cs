namespace CIS.Core.Security;

/// <summary>
/// Informace o technickem uzivateli pod kterym je vytvoren request na interni sluzbu
/// </summary>
public interface IServiceUser
{
    /// <summary>
    /// Login technickeho uzivatele
    /// </summary>
    string? Name { get; }
}
