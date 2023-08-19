using Microsoft.AspNetCore.Mvc.Filters;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Infrastructure.AuditLog;

public class AuditLogAttribute : Attribute, IActionFilter
{
    public const string LogHttpRequestAndResponseKey = "LogHttpRequestAndResponse";
    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items["LogHttpRequestAndResponse"] = true;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}