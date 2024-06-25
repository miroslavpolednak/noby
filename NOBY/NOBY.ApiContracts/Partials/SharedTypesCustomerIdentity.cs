using SharedTypes.GrpcTypes;

namespace NOBY.ApiContracts;

public partial class SharedTypesCustomerIdentity
{
    public static implicit operator SharedTypesCustomerIdentity?(Identity? identity)
    {
        if (identity is null) return null;

        return new SharedTypesCustomerIdentity
        {
            Scheme = (SharedTypesCustomerIdentityScheme)identity.IdentityScheme,
            Id = identity.IdentityId
        };
    }

    public static implicit operator Identity(SharedTypesCustomerIdentity identity)
    {
        ArgumentNullException.ThrowIfNull(identity);

        return new Identity()
        {
            IdentityId = identity.Id,
            IdentityScheme = (Identity.Types.IdentitySchemes)identity.Scheme,
        };
    }

    public static bool operator ==(SharedTypesCustomerIdentity? left, SharedTypesCustomerIdentity? right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null)
            return false;

        if (right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(SharedTypesCustomerIdentity? left, SharedTypesCustomerIdentity? right)
    {
        return !(left == right);
    }
}
