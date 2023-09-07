namespace CIS.InternalServices.DataAggregatorService.Api.Extensions;

public static class IdentityExtensions
{
    public static Identity GetIdentity(this IEnumerable<Identity> identities, Identity.Types.IdentitySchemes preferScheme)
    {
        return identities.FirstOrDefault(i => i.IdentityScheme == preferScheme, identities.First());
    }
}