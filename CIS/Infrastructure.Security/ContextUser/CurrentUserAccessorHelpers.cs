namespace CIS.Infrastructure.Security;

public static class CurrentUserAccessorHelpers
{
    public static int? GetUserIdFromHeaders(HttpRequest request)
    {
        int? partyId = null;
        if (request.Headers.ContainsKey(Core.Security.SecurityConstants.ContextUserHttpHeaderUserIdKey)
            && int.TryParse(request.Headers[Core.Security.SecurityConstants.ContextUserHttpHeaderUserIdKey].First(), out int i))
            partyId = i;
        return partyId;
    }

    public static string? GetUserIdentFromHeaders(HttpRequest request)
    {
        return request.Headers.ContainsKey(Core.Security.SecurityConstants.ContextUserHttpHeaderUserIdentKey) ? request.Headers[Core.Security.SecurityConstants.ContextUserHttpHeaderUserIdentKey].First() : null;
    }
}
