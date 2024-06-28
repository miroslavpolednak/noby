namespace NOBY.ApiContracts;

public partial class SharedTypesUserIdentity
{
    public static implicit operator SharedTypesUserIdentity?(SharedTypes.GrpcTypes.UserIdentity? identity)
    {
        if (identity is null) return null;

        return new SharedTypesUserIdentity
        {
            Scheme = (SharedTypesUserIdentityScheme)identity.IdentityScheme,
            Identity = identity.Identity
        };
    }

    public static implicit operator SharedTypes.GrpcTypes.UserIdentity(SharedTypesUserIdentity identity)
    {
        ArgumentNullException.ThrowIfNull(identity);

        return new SharedTypes.GrpcTypes.UserIdentity()
        {
            Identity = identity.Identity,
            IdentityScheme = (SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes)identity.Scheme,
        };
    }
}
