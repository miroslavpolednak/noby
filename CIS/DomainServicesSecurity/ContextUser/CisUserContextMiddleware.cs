using CIS.Core.Results;

namespace CIS.DomainServicesSecurity.ContextUser;

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
            var userService = httpContext.RequestServices.GetRequiredService<DomainServices.UserService.Abstraction.IUserServiceAbstraction>();
            var userInstance = ServiceCallResult.ResolveToDefault<DomainServices.UserService.Contracts.User>(await userService.GetUser(partyId.Value));

            // pridat identitu
            if (userInstance is not null)
            {
                httpContext.User.AddIdentity(new CisUserIdentity(userInstance));
                _logger.ContextUserAdded(partyId.Value);
            }
        }

        await _next(httpContext);
    }
}
