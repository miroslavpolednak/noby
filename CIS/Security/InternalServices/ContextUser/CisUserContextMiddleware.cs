using System.Diagnostics;

namespace CIS.Security.InternalServices.ContextUser;

internal sealed class CisUserContextMiddleware
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
        int? partyId = null;
        if (int.TryParse(Activity.Current?.Baggage.FirstOrDefault(b => b.Key == CisUserContextHelpers.ContextUserBaggageKey).Value, out int i))
            partyId = i;

        if (partyId.HasValue)
        {
            _logger.ContextUserAdded(partyId.Value);

            // pridat identitu
            httpContext.User.AddIdentity(new CisUserIdentity(partyId.Value, ""));
        }

        await _next(httpContext);
    }
}
