namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class Identity
{
    public Identity(Core.Types.CustomerIdentity identity)
    {
        IdentityId = identity.Id;
        IdentityScheme = Enum.Parse<IdentitySchemes>(identity.Scheme.ToString());
    }

    public Identity(int? identityId, Core.IdentitySchemes? scheme)
    {
        IdentityId = identityId ?? 0;
        IdentityScheme = Enum.Parse<IdentitySchemes>((scheme ?? Core.IdentitySchemes.Unknown).ToString());
    }

    public static implicit operator Core.Types.CustomerIdentity(Identity identity)
    {
        if (identity is null) throw new ArgumentNullException(nameof(identity), "CustomerIdentity is null");
        return new Core.Types.CustomerIdentity(identity.IdentityId, identity.IdentityScheme.ToString());
    }

    public static implicit operator Identity(Core.Types.CustomerIdentity identity)
    {
        if (identity is null) throw new ArgumentNullException(nameof(identity), "CustomerIdentity is null");
        return new Identity()
        {
            IdentityId = identity.Id,
            IdentityScheme = Enum.Parse<IdentitySchemes>(identity.Scheme.ToString()),
        };
    }
}
