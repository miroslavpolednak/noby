namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class UserIdentity
{
    public UserIdentity(Foms.Types.UserIdentity identity)
    {
        Identity = identity.Identity;
        IdentityScheme = Enum.Parse<Types.UserIdentitySchemes>(identity.Scheme.ToString());
    }

    public UserIdentity(string? identity, Foms.Enums.UserIdentitySchemes? scheme)
    {
        Identity = identity;
        IdentityScheme = Enum.Parse<Types.UserIdentitySchemes>((scheme ?? Foms.Enums.UserIdentitySchemes.Unknown).ToString());
    }

    public static implicit operator Foms.Types.UserIdentity?(UserIdentity? identity)
    {
        if (identity is null) return null;
        //if (identity is null) throw new ArgumentNullException(nameof(identity), "CustomerIdentity is null");
        return new Foms.Types.UserIdentity(identity.Identity, identity.IdentityScheme.ToString());
    }

    public static implicit operator UserIdentity(Foms.Types.UserIdentity identity)
    {
        if (identity is null) throw new ArgumentNullException(nameof(identity), "UserIdentity is null");
        return new UserIdentity()
        {
            Identity = identity.Identity,
            IdentityScheme = Enum.Parse<Types.UserIdentitySchemes>(identity.Scheme.ToString()),
        };
    }
}
