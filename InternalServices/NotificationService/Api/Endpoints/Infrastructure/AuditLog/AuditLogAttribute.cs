using CIS.InternalServices.NotificationService.Api.Services.AuditLog.Abstraction;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Infrastructure.AuditLog;

public class AuditLogAttribute : Attribute, IExceptionFilter
{
    public void OnActionExecuted(ActionExecutedContext context) 
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ISmsAuditLogger>();
        logger.LogHttpRequestProcessed(context.Result).GetAwaiter();
    }

    public void OnException(ExceptionContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ISmsAuditLogger>();
        logger.LogHttpRequestError(context.Exception).GetAwaiter();
    }
}