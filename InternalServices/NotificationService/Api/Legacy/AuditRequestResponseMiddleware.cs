using CIS.InternalServices.NotificationService.Api.Endpoints.v1;
using CIS.InternalServices.NotificationService.Api.Legacy.AuditLog.Abstraction;

namespace CIS.InternalServices.NotificationService.Api.Legacy;

public class AuditRequestResponseMiddleware
{
    private readonly RequestDelegate _next;

    public AuditRequestResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Request.EnableBuffering();
        await _next(context);

        var enabled = context.Items[AuditLogAttribute.LogHttpRequestAndResponseKey];

        if (enabled is true)
        {
            var logger = context.RequestServices.GetRequiredService<ISmsAuditLogger>();
            await logger.LogHttpRequestResponse();
        }
    }

}