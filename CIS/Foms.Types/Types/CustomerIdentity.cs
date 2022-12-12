using CIS.Core.Exceptions;
using CIS.Foms.Enums;

namespace CIS.Foms.Types;

/// <summary>
/// Identita klienta
/// </summary>
public sealed class CustomerIdentity
{
    /// <summary>
    /// ID klienta v danem schematu
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Schema ve kterem je klient ulozeny - Kb | Mp
    /// </summary>
    public IdentitySchemes Scheme { get; init; }

    public CustomerIdentity()
    {
    }

    public CustomerIdentity(long id, IdentitySchemes scheme)
    {
        Id = id;
        Scheme = scheme;
    }

    public CustomerIdentity(long id, int scheme)
    {
        Id = id;
        Scheme = (IdentitySchemes)scheme;
    }

    public CustomerIdentity(long? id, string? scheme) 
        : this(id.GetValueOrDefault(), scheme) { }

    public CustomerIdentity(long id, string? scheme)
    {
        if (!Enum.TryParse(scheme, out IdentitySchemes parsedScheme))
            throw new CisArgumentException(1, "CustomerIdentity scheme is not in valid format", nameof(scheme));

        Id = id;
        Scheme = parsedScheme;
    }

    public CustomerIdentity(string token)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(token);

        int idx = token.IndexOf(':');
        if (idx < 1)
            throw new CisArgumentException(1, "CustomerIdentity token is not in valid format", nameof(token));
        if (!long.TryParse(token.AsSpan(idx + 1), out long id))
            throw new CisArgumentException(1, "CustomerIdentity token is not in valid format", nameof(token));
        if (!Enum.TryParse(token.Substring(0, idx), out IdentitySchemes parsedScheme))
            throw new CisArgumentException(1, "CustomerIdentity scheme is not in valid format", nameof(token));

        Id = id;
        Scheme = parsedScheme;
    }
}
