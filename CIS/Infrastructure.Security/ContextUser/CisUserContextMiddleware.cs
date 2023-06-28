namespace CIS.Infrastructure.Security.ContextUser;

internal sealed class CisUserContextMiddleware
{
    private readonly ILogger<CisUserContextMiddleware> _logger;
    private readonly RequestDelegate _next;

    public CisUserContextMiddleware(
        RequestDelegate next, 
        ILoggerFactory logFactory)
    {
        _logger = logFactory.CreateLogger<CisUserContextMiddleware>();
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        int? partyId = CurrentUserAccessorHelpers.GetUserIdFromHeaders(httpContext.Request);

        if (partyId.HasValue)
        {
            // vytvorit identitu
            var identity = new CisUserIdentity(partyId.Value, CurrentUserAccessorHelpers.GetUserIdentFromHeaders(httpContext.Request));
            httpContext.User.AddIdentity(identity);

            _logger.ContextUserAdded(partyId.Value);
        }

        await _next(httpContext);
    }
}
