using CIS.Core.Security;
using CIS.Infrastructure.Security.ContextUser;

namespace CIS.Infrastructure.Security;

public static class CurrentUserAccessorHelpers
{
    public static int? GetUserIdFromHeaders(HttpRequest request)
    {
        int? partyId = null;
        if (request.Headers.ContainsKey(SecurityConstants.ContextUserHttpHeaderUserIdKey)
            && int.TryParse(request.Headers[SecurityConstants.ContextUserHttpHeaderUserIdKey].First(), out int i))
            partyId = i;
        return partyId;
    }

    public static string? GetUserIdentFromHeaders(HttpRequest request)
    {
        return request.Headers.ContainsKey(SecurityConstants.ContextUserHttpHeaderUserIdentKey) 
            ? request.Headers[SecurityConstants.ContextUserHttpHeaderUserIdentKey].First() : null;
    }

    public static CIS.Foms.Types.UserIdentity? GetUserIdentityFromHeaders(HttpRequest? request)
    {
        if (request is null)
        {
            return null;
        }

        var ident = GetUserIdentFromHeaders(request);

        if (ident == null) return null;
        int idx = ident.IndexOf('=', 0);

        return new Foms.Types.UserIdentity
        {
            Scheme = FastEnumUtility.FastEnum.Parse<Foms.Enums.UserIdentitySchemes>(ident[..idx], true),
            Identity = ident[(idx + 1)..]
        };
    }

    public static CIS.Foms.Types.UserIdentity? GetUserIdentityFromHeaders(this ICurrentUserAccessor currentUserAccessor)
    {
        var accessor = currentUserAccessor as CisCurrentContextUserAccessor;
        if (accessor == null) return null;
        return accessor.GetMainIdentity();
    }
}
