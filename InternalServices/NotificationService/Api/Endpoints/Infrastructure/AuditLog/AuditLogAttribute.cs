using CIS.InternalServices.NotificationService.Api.Services.AuditLog;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Infrastructure.AuditLog;

public class AuditLogAttribute : Attribute, IActionFilter, IExceptionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context) 
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<SmsAuditLogger>();
        logger.LogHttpResponse(context.Result).GetAwaiter();
    }

    public void OnException(ExceptionContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<SmsAuditLogger>();
        logger.LogHttpException(context.Exception);
    }
}