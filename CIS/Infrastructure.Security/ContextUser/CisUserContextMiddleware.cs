namespace CIS.Infrastructure.Security.ContextUser;

internal sealed class CisUserContextMiddleware
{
    private readonly ILogger<CisUserContextMiddleware> _logger;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;
    private readonly RequestDelegate _next;

    public CisUserContextMiddleware(
        RequestDelegate next, 
        ILoggerFactory logFactory, 
        DomainServices.UserService.Clients.IUserServiceClient userService)
    {
        _logger = logFactory.CreateLogger<CisUserContextMiddleware>();
        _next = next;
        _userService = userService;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        int? partyId = CurrentUserAccessorHelpers.GetUserIdFromHeaders(httpContext.Request);

        if (partyId.HasValue)
        {
            // ziskat detail uzivatele z user service
            var userInstance = await _userService.GetUser(partyId.Value);

            // vytvorit identitu
            var identity = new CisUserIdentity(CurrentUserAccessorHelpers.GetUserIdentFromHeaders(httpContext.Request), userInstance);
            httpContext.User.AddIdentity(identity);

            _logger.ContextUserAdded(partyId.Value);
        }

        await _next(httpContext);
    }
}
