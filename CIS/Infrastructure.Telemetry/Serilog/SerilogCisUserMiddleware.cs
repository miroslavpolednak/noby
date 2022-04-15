using CIS.Core.Security;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace CIS.Infrastructure.Telemetry.Serilog;

/// <summary>
/// Middleware, ktery pridava do kontextu logu (serilogu) informace o aktualnim CIS uzivateli
/// </summary>
public class SerilogCisUserMiddleware
{
    private readonly RequestDelegate _next;

    public SerilogCisUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ICurrentUserAccessor userAccessor)
    {
        if (userAccessor?.User is not null)
        {
            using (LogContext.PushProperty("CisUserId", userAccessor.User.Id))
            {
                await _next.Invoke(context);
            }
        }
    }
}
