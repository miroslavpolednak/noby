namespace CIS.Infrastructure.gRPC.CisTypes;

public partial class Identity
{
    public Identity(SharedTypes.Types.CustomerIdentity identity)
    {
        IdentityId = identity.Id;
        IdentityScheme = FastEnum.Parse<Types.IdentitySchemes>(identity.Scheme.ToString());
    }

    public Identity(long? identityId, SharedTypes.Enums.IdentitySchemes? scheme)
    {
        IdentityId = identityId ?? 0;
        IdentityScheme = FastEnum.Parse<Types.IdentitySchemes>((scheme ?? SharedTypes.Enums.IdentitySchemes.Unknown).ToString());
    }

    public static implicit operator SharedTypes.Types.CustomerIdentity?(Identity? identity)
    {
        if (identity is null) return null;
        
        return new SharedTypes.Types.CustomerIdentity(identity.IdentityId, identity.IdentityScheme.ToString());
    }

    public static implicit operator Identity(SharedTypes.Types.CustomerIdentity identity)
    {
        ArgumentNullException.ThrowIfNull(identity);

        return new Identity()
        {
            IdentityId = identity.Id,
            IdentityScheme = FastEnum.Parse<Types.IdentitySchemes>(identity.Scheme.ToString()),
        };
    }

    public static bool operator ==(Identity? left, Identity? right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null)
            return false;

        if (right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Identity? left, Identity? right)
    {
        return !(left == right);
    }
}
