namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class Identity
{
    public Identity(CIS.Foms.Types.CustomerIdentity identity)
    {
        IdentityId = identity.Id;
        IdentityScheme = Enum.Parse<Types.IdentitySchemes>(identity.Scheme.ToString());
    }

    public Identity(int? identityId, CIS.Foms.Enums.IdentitySchemes? scheme)
    {
        IdentityId = identityId ?? 0;
        IdentityScheme = Enum.Parse<Types.IdentitySchemes>((scheme ?? CIS.Foms.Enums.IdentitySchemes.Unknown).ToString());
    }

    public static implicit operator CIS.Foms.Types.CustomerIdentity(Identity identity)
    {
        if (identity is null) throw new ArgumentNullException(nameof(identity), "CustomerIdentity is null");
        return new CIS.Foms.Types.CustomerIdentity(identity.IdentityId, identity.IdentityScheme.ToString());
    }

    public static implicit operator Identity(CIS.Foms.Types.CustomerIdentity identity)
    {
        if (identity is null) throw new ArgumentNullException(nameof(identity), "CustomerIdentity is null");
        return new Identity()
        {
            IdentityId = identity.Id,
            IdentityScheme = Enum.Parse<Types.IdentitySchemes>(identity.Scheme.ToString()),
        };
    }
}
