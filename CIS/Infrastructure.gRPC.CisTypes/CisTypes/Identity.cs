namespace CIS.Infrastructure.gRPC.CisTypes;

public partial class Identity
{
    public Identity(Foms.Types.CustomerIdentity identity)
    {
        IdentityId = identity.Id;
        IdentityScheme = FastEnum.Parse<Types.IdentitySchemes>(identity.Scheme.ToString());
    }

    public Identity(long? identityId, Foms.Enums.IdentitySchemes? scheme)
    {
        IdentityId = identityId ?? 0;
        IdentityScheme = FastEnum.Parse<Types.IdentitySchemes>((scheme ?? Foms.Enums.IdentitySchemes.Unknown).ToString());
    }

    public static implicit operator Foms.Types.CustomerIdentity?(Identity? identity)
    {
        if (identity is null) return null;
        
        return new Foms.Types.CustomerIdentity(identity.IdentityId, identity.IdentityScheme.ToString());
    }

    public static implicit operator Identity(Foms.Types.CustomerIdentity identity)
    {
        ArgumentNullException.ThrowIfNull(identity);

        return new Identity()
        {
            IdentityId = identity.Id,
            IdentityScheme = FastEnum.Parse<Types.IdentitySchemes>(identity.Scheme.ToString()),
        };
    }
}
