using CIS.Core.Exceptions;
using CIS.Foms.Enums;

namespace CIS.Foms.Types;

public class CustomerIdentity
{
    public int Id { get; init; }
    public IdentitySchemes Scheme { get; init; }

    public CustomerIdentity()
    {
    }

    public CustomerIdentity(int id, IdentitySchemes scheme)
    {
        Id = id;
        Scheme = scheme;
    }

    public CustomerIdentity(int id, int scheme)
    {
        Id = id;
        Scheme = (IdentitySchemes)scheme;
    }

    public CustomerIdentity(int? id, string? scheme) 
        : this(id.GetValueOrDefault(), scheme) { }

    public CustomerIdentity(int id, string? scheme)
    {
        Id = id;
        if (!Enum.TryParse(scheme, out Enums.IdentitySchemes parsedScheme))
            throw new CisArgumentException(1, "CustomerIdentity scheme is not in valid format", nameof(scheme));
        Scheme = parsedScheme;
    }

    public CustomerIdentity(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new CisArgumentNullException(1, "CustomerIdentity token is null or empty", nameof(token));

        int idx = token.IndexOf(':');
        if (idx < 1)
            throw new CisArgumentException(1, "CustomerIdentity token is not in valid format", nameof(token));
        if (!int.TryParse(token.AsSpan(idx + 1), out int id))
            throw new CisArgumentException(1, "CustomerIdentity token is not in valid format", nameof(token));
        if (!Enum.TryParse(token.Substring(0, idx), out Enums.IdentitySchemes parsedScheme))
            throw new CisArgumentException(1, "CustomerIdentity scheme is not in valid format", nameof(token));

        Id = id;
        Scheme = parsedScheme;
    }
}
