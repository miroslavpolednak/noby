using CIS.InternalServices.NotificationService.Api.Services.AuditLog.Abstraction;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Infrastructure.AuditLog;

public class AuditLogAttribute : Attribute, IActionFilter, IExceptionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ISmsAuditLogger>();
        logger.LogHttpRequest().GetAwaiter();
    }

    public void OnActionExecuted(ActionExecutedContext context) 
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ISmsAuditLogger>();
        logger.LogHttpResponse(context.Result).GetAwaiter();
    }

    public void OnException(ExceptionContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ISmsAuditLogger>();
        logger.LogHttpException(context.Exception);
    }
}