using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace CIS.Infrastructure.Telemetry.Middlewares;

internal sealed class LoggerCisUserGrpcMiddleware
{
    private readonly RequestDelegate _next;

    public LoggerCisUserGrpcMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        int? userId = DomainServicesSecurity.CurrentUserAccessorHelpers.GetUserIdFromHeaders(context.Request);
        if (userId.HasValue)
        {
            using (LogContext.PushProperty(Constants.LoggerContextUserIdPropertyName, userId.Value))
            {
                await _next.Invoke(context);
            }
        }
        else
            await _next.Invoke(context);
    }
}
