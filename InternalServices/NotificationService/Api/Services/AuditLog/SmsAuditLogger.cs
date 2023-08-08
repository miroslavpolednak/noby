using System.Text;
using CIS.Infrastructure.Audit;
using CIS.InternalServices.NotificationService.Api.Services.AuditLog.Abstraction;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using DomainServices.CodebookService.Contracts.v1;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIS.InternalServices.NotificationService.Api.Services.AuditLog;

public class SmsAuditLogger : ISmsAuditLogger
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuditLogger _auditLogger;

    public SmsAuditLogger(
        IHttpContextAccessor httpContextAccessor,
        IAuditLogger auditLogger)
    {
        _httpContextAccessor = httpContextAccessor;
        _auditLogger = auditLogger;
    }

    private async Task<string> GetBodyFromRequest(HttpRequest request, Encoding? encoding = null)
    {
        request.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
        var requestBody = await streamReader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        return requestBody;
    }

    private async Task<string> GetBodyFromResponse(HttpResponse response)
    {
        using var memoryStream = new MemoryStream();
        var original = response.Body;
        response.Body = memoryStream;

        memoryStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
        memoryStream.Seek(0, SeekOrigin.Begin);

        await memoryStream.CopyToAsync(original);
        response.Body = original;

        return responseBody;
    }

    public async Task LogHttpRequest()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null) return;

        var rawHttpRequestBody = await GetBodyFromRequest(httpContext.Request);
        
        _auditLogger.LogWithCurrentUser(
            AuditEventTypes.Noby012,
            "Received HTTP Request",
            bodyBefore: new Dictionary<string, string>
            {
                { "rawHttpRequestBody", rawHttpRequestBody }
            }
        );
    }

    public async Task LogHttpResponse(IActionResult? actionResult)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null) return;

        var rawHttpResponseBody = await GetBodyFromResponse(httpContext.Response);
        
        if (actionResult is ObjectResult result)
        {
            rawHttpResponseBody = JsonConvert.SerializeObject(result.Value);
        }
        
        _auditLogger.LogWithCurrentUser(
            AuditEventTypes.Noby012,
            "Sending HTTP Response",
            bodyAfter: new Dictionary<string, string>
            {
                { "rawHttpResponseBody", rawHttpResponseBody }
            }
        );
    }

    public void LogHttpException(Exception exception)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null) return;
        
        _auditLogger.LogWithCurrentUser(
            AuditEventTypes.Noby012,
            "Sending HTTP Exception",
            bodyAfter: new Dictionary<string, string>
            {
                { "exception", JsonConvert.SerializeObject(exception) }
            }
        );
    }
    
    public void LogKafkaProduced(SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, Guid notificationId, string consumer)
    {
        if (smsType.IsAuditLogEnabled)
        {
            _auditLogger.LogWithCurrentUser(
                AuditEventTypes.Noby013,
                "Produced message SendSMS to KAFKA",
                bodyBefore: new Dictionary<string, string>
                {
                    { "smsType", smsType.Code },
                    { "consumer", consumer }
                },
                bodyAfter: new Dictionary<string, string>
                {
                    { "notificationId", notificationId.ToString() }
                });
        }
    }

    public void LogKafkaError(SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, string consumer, string errorMessage)
    {
        if (smsType.IsAuditLogEnabled)
        {
            _auditLogger.LogWithCurrentUser(
                AuditEventTypes.Noby013,
                "Could not produce message SendSMS to KAFKA",
                bodyBefore: new Dictionary<string, string>
                {
                    { "smsType", smsType.Code },
                    { "consumer", consumer }
                },
                bodyAfter: new Dictionary<string, string>
                {
                    { "errorMessage", errorMessage }
                });
        }
    }

    public void LogKafkaResultReceived(SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, NotificationReport report)
    {
        if (smsType.IsAuditLogEnabled)
        {
            _auditLogger.Log(
                AuditEventTypes.Noby013,
                "Received notification report for sms",
                bodyBefore: new Dictionary<string, string>
                {
                    { "smsType", smsType.Code },
                    { "id", report.id },
                    { "state", report.state },
                    { "errors", JsonConvert.SerializeObject(report.notificationErrors) }
                });
        }
    }
}