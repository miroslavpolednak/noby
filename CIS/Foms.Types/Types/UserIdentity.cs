using CIS.Core.Exceptions;
using CIS.Foms.Enums;

namespace CIS.Foms.Types;

/// <summary>
/// Identita uživatele NOBY aplikace
/// </summary>
public sealed class UserIdentity
{
    /// <summary>
    /// ID uživatele
    /// </summary>
    public string Identity { get; init; }
    
    /// <summary>
    /// Identitní schéma
    /// </summary>
    public UserIdentitySchemes Scheme { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public UserIdentity()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    /// <param name="identity">ID uživatele</param>
    /// <param name="scheme">Identitní schéma</param>
    /// <exception cref="CisArgumentException">ID není zadáno</exception>
    public UserIdentity(string identity, UserIdentitySchemes scheme)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(identity);

        Identity = identity;
        Scheme = scheme;
    }

    public UserIdentity(string identity, int scheme)
        : this(identity, (UserIdentitySchemes)scheme)
    { }

    /// <param name="identity">ID uživatele</param>
    /// <param name="scheme">Identitní schéma</param>
    /// <exception cref="CisArgumentException">ID nebo schéma není zadáno</exception>
    public UserIdentity(string? identity, string? scheme)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(identity);
        if (!Enum.TryParse(scheme, out UserIdentitySchemes parsedScheme))
            throw new ArgumentException("UserIdentity scheme is not in valid format", nameof(scheme));

        Identity = identity;
        Scheme = parsedScheme;
    }
}
