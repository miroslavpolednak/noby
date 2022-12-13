namespace CIS.Infrastructure.gRPC.CisTypes;

public partial class UserIdentity
{
    public UserIdentity(Foms.Types.UserIdentity identity)
    {
        Identity = identity.Identity;
        IdentityScheme = FastEnum.Parse<Types.UserIdentitySchemes>(identity.Scheme.ToString());
    }

    public UserIdentity(string? identity, Foms.Enums.UserIdentitySchemes? scheme)
    {
        Identity = identity;
        IdentityScheme = FastEnum.Parse<Types.UserIdentitySchemes>((scheme ?? Foms.Enums.UserIdentitySchemes.Unknown).ToString());
    }

    public static implicit operator Foms.Types.UserIdentity?(UserIdentity? identity)
    {
        if (identity is null) return null;
        
        return new Foms.Types.UserIdentity(identity.Identity, identity.IdentityScheme.ToString());
    }

    public static implicit operator UserIdentity(Foms.Types.UserIdentity identity)
    {
        ArgumentNullException.ThrowIfNull(identity);
        
        return new UserIdentity()
        {
            Identity = identity.Identity,
            IdentityScheme = FastEnum.Parse<Types.UserIdentitySchemes>(identity.Scheme.ToString()),
        };
    }
}
