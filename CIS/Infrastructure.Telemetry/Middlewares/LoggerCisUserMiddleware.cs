using CIS.Core.Security;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace CIS.Infrastructure.Telemetry.Middlewares;

/// <summary>
/// Middleware, ktery pridava do kontextu logu (serilogu) informace o aktualnim CIS uzivateli
/// </summary>
internal sealed class LoggerCisUserMiddleware
{
    private readonly RequestDelegate _next;

    public LoggerCisUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var userAccessor = context.RequestServices.GetService(typeof(ICurrentUserAccessor)) as ICurrentUserAccessor;
        if (userAccessor is not null && userAccessor.IsAuthenticated)
        {
            using (LogContext.PushProperty("CisUserId", userAccessor.User!.Id))
            {
                await _next.Invoke(context);
            }
        }
        else
            await _next.Invoke(context);
    }
}
