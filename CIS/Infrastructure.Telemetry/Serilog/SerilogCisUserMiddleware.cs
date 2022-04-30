using CIS.Core.Security;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Diagnostics;

namespace CIS.Infrastructure.Telemetry.Serilog;

/// <summary>
/// Middleware, ktery pridava do kontextu logu (serilogu) informace o aktualnim CIS uzivateli
/// </summary>
public class SerilogCisUserMiddleware
{
    const string ContextUserBaggageKey = "MpPartyId";

    private readonly RequestDelegate _next;

    public SerilogCisUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // pokud se posila v ramci headeru requestu
        if (int.TryParse(Activity.Current?.Baggage.FirstOrDefault(b => b.Key == ContextUserBaggageKey).Value, out int id))
        {
            using (LogContext.PushProperty("CisUserId", id))
            {
                await _next.Invoke(context);
            }
        }
        // jinak pokud je v httpContextu
        else
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
}
