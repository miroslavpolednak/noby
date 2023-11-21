using CIS.Core.Security;
using CIS.Infrastructure.Security.ContextUser;
using Microsoft.Extensions.Primitives;

namespace CIS.Infrastructure.Security;

public static class CurrentUserAccessorHelpers
{
    public static int? GetUserIdFromHeaders(HttpRequest request)
    {
        int? partyId = null;
        if (request.Headers.TryGetValue(SecurityConstants.ContextUserHttpHeaderUserIdKey, out StringValues value) && int.TryParse(value[0], out int i))
        {
            partyId = i;
        }
        return partyId;
    }

    public static string? GetUserIdentFromHeaders(HttpRequest request)
    {
        return request.Headers.TryGetValue(SecurityConstants.ContextUserHttpHeaderUserIdentKey, out StringValues value) ? value[0] : null;
    }

    public static SharedTypes.Types.UserIdentity? GetUserIdentityFromHeaders(HttpRequest? request)
    {
        if (request is null)
        {
            return null;
        }

        var ident = GetUserIdentFromHeaders(request);

        if (ident == null) return null;
        int idx = ident.IndexOf('=', 0);

        return new SharedTypes.Types.UserIdentity
        {
            Scheme = FastEnumUtility.FastEnum.Parse<SharedTypes.Enums.UserIdentitySchemes>(ident[..idx], true),
            Identity = ident[(idx + 1)..]
        };
    }

    public static SharedTypes.Types.UserIdentity? GetUserIdentityFromHeaders(this ICurrentUserAccessor currentUserAccessor)
    {
        var accessor = currentUserAccessor as CisCurrentContextUserAccessor;
        if (accessor == null) return null;
        return accessor.GetMainIdentity();
    }
}
