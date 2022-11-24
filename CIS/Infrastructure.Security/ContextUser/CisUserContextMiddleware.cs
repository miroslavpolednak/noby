namespace CIS.Infrastructure.Security.ContextUser;

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
        int? partyId = CurrentUserAccessorHelpers.GetUserIdFromHeaders(httpContext.Request);
        if (partyId.HasValue)
        {
            httpContext.User.AddIdentity(new CisUserIdentity(partyId.Value));
            _logger.ContextUserAdded(partyId.Value);
        }

        await _next(httpContext);
    }
}
