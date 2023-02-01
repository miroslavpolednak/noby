using CIS.Core;
using CIS.Infrastructure.Telemetry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Infrastructure.AuditLog;

public class AuditLogAttribute : Attribute, IActionFilter, IExceptionFilter
{
    private string GetBodyFromRequest(HttpRequest request)
    {
        request.EnableBuffering();
        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
        var requestBody = streamReader.ReadToEndAsync().GetAwaiter().GetResult();
        request.Body.Seek(0, SeekOrigin.Begin);
        return requestBody;
    }

    private string GetBodyFromResponse(HttpResponse response)
    {
        using var memoryStream = new MemoryStream();
        var original = response.Body;
        response.Body = memoryStream;
            
        memoryStream.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(memoryStream).ReadToEnd();
        memoryStream.Seek(0, SeekOrigin.Begin);

        memoryStream.CopyToAsync(original).GetAwaiter().GetResult();
        response.Body = original;

        return responseBody;
    }
    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var auditLogger = httpContext.RequestServices.GetRequiredService<IAuditLogger>();
        var dateTime = httpContext.RequestServices.GetRequiredService<IDateTime>();
        var log = new AuditHttpRequestLog();
        log.TraceId = httpContext.TraceIdentifier;
        log.RequestDateTimeUtc = dateTime.Now;
        
        log.ClientIp = httpContext.Connection.RemoteIpAddress?.ToString();
        log.RequestMethod = httpContext.Request.Method;
        log.RequestPath = httpContext.Request.Path;
        log.RequestQuery = httpContext.Request.QueryString.ToString();
        log.RequestHeaders = httpContext.Request.Headers.ToDictionary(v => v.Key, v => v.Value);
        log.RequestBody = GetBodyFromRequest(httpContext.Request);
        log.RequestScheme = httpContext.Request.Scheme;
        log.RequestHost = httpContext.Request.Host.ToString();
        log.RequestContentType = httpContext.Request.ContentType;
        
        var logMessage = JsonConvert.SerializeObject(log);
        auditLogger.Log(logMessage);
    }

    public void OnActionExecuted(ActionExecutedContext context) 
    {
        var httpContext = context.HttpContext;
        var auditLogger = httpContext.RequestServices.GetRequiredService<IAuditLogger>();
        var dateTime = httpContext.RequestServices.GetRequiredService<IDateTime>();
        var log = new AuditHttpResponseLog();
        log.ResponseDateTimeUtc = dateTime.Now;
        log.TraceId = httpContext.TraceIdentifier;

        log.ResponseContentType = httpContext.Response.ContentType;
        log.ResponseStatus = httpContext.Response.StatusCode.ToString();
        log.ResponseHeaders = httpContext.Response.Headers.ToDictionary(v => v.Key, v => v.Value);
        log.ResponseBody =  GetBodyFromResponse(httpContext.Response);

        if (context.Result is ObjectResult result)
        {
            log.ResponseStatus = result.StatusCode?.ToString() ?? string.Empty;
            log.ResponseBody = JsonConvert.SerializeObject(result.Value);
        }
        
        var logMessage = JsonConvert.SerializeObject(log);
        auditLogger.Log(logMessage);
    }

    public void OnException(ExceptionContext context)
    {
        var httpContext = context.HttpContext;
        var auditLogger = httpContext.RequestServices.GetRequiredService<IAuditLogger>();
        var dateTime = httpContext.RequestServices.GetRequiredService<IDateTime>();

        var log = new AuditHttpExceptionLog();
        log.TraceId = httpContext.TraceIdentifier;
        log.ResponseDateTimeUtc = dateTime.Now;
        log.ResponseStatus = httpContext.Response.StatusCode.ToString();
        log.Exception = context.Exception;
        
        var logMessage = JsonConvert.SerializeObject(log);
        auditLogger.Log(logMessage);
    }
}