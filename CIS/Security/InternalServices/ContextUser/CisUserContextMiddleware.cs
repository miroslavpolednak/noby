using System.Diagnostics;

namespace CIS.Security.InternalServices.ContextUser;

public sealed class CisUserContextMiddleware
{
    private readonly ILogger<CisUserContextMiddleware> _logger;
    private readonly RequestDelegate _next;

    public CisUserContextMiddleware(RequestDelegate next, ILoggerFactory logFactory)
    {
        _logger = logFactory.CreateLogger<CisUserContextMiddleware>();
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var partyId = Activity.Current?.Baggage.FirstOrDefault(b => b.Key == "MpPartyId").Value;
        if (!string.IsNullOrEmpty(partyId))
        {
            _logger.ContextUserAdded(partyId);

            // pridat identitu
            httpContext.User.AddIdentity(new CisUserIdentity(1, "", ""));
        }

        await _next(httpContext);
    }
}
