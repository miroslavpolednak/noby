using Microsoft.AspNetCore.Http;

namespace CIS.DomainServicesSecurity;

public static class CurrentUserAccessorHelpers
{
    public static int? GetUserIdFromHeaders(HttpRequest request)
    {
        int? partyId = null;
        if (request.Headers.ContainsKey(Core.Security.Constants.ContextUserHttpHeaderKey)
            && int.TryParse(request.Headers[Core.Security.Constants.ContextUserHttpHeaderKey], out int i))
            partyId = i;
        return partyId;
    }
}
