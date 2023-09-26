namespace CIS.Infrastructure.gRPC.CisTypes;

public partial class UserIdentity
{
    public UserIdentity(SharedTypes.Types.UserIdentity identity)
    {
        Identity = identity.Identity;
        IdentityScheme = FastEnum.Parse<Types.UserIdentitySchemes>(identity.Scheme.ToString());
    }

    public UserIdentity(string? identity, SharedTypes.Enums.UserIdentitySchemes? scheme)
    {
        Identity = identity;
        IdentityScheme = FastEnum.Parse<Types.UserIdentitySchemes>((scheme ?? SharedTypes.Enums.UserIdentitySchemes.Unknown).ToString());
    }

    public static implicit operator SharedTypes.Types.UserIdentity?(UserIdentity? identity)
    {
        if (identity is null) return null;
        
        return new SharedTypes.Types.UserIdentity(identity.Identity, identity.IdentityScheme.ToString());
    }

    public static implicit operator UserIdentity(SharedTypes.Types.UserIdentity identity)
    {
        ArgumentNullException.ThrowIfNull(identity);
        
        return new UserIdentity()
        {
            Identity = identity.Identity,
            IdentityScheme = FastEnum.Parse<Types.UserIdentitySchemes>(identity.Scheme.ToString()),
        };
    }
}
