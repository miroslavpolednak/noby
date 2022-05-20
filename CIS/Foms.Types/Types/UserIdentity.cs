using CIS.Core.Exceptions;
using CIS.Foms.Enums;

namespace CIS.Foms.Types;

public sealed class UserIdentity
{
    public string Identity { get; init; }
    public UserIdentitySchemes Scheme { get; init; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public UserIdentity()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public UserIdentity(string identity, UserIdentitySchemes scheme)
    {
        if (string.IsNullOrEmpty(identity))
            throw new CisArgumentException(1, "UserIdentity identity is empty", nameof(identity));
        Identity = identity;
        Scheme = scheme;
    }

    public UserIdentity(string identity, int scheme)
        : this(identity, (UserIdentitySchemes)scheme)
    { }

    public UserIdentity(string? identity, string? scheme)
    {
        if (string.IsNullOrEmpty(identity))
            throw new CisArgumentException(1, "UserIdentity identity is empty", nameof(identity));
        Identity = identity;
        if (!Enum.TryParse(scheme, out UserIdentitySchemes parsedScheme))
            throw new CisArgumentException(1, "UserIdentity scheme is not in valid format", nameof(scheme));
        Scheme = parsedScheme;
    }
}
