using CIS.Core;
using CIS.Core.Attributes;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using DomainServices.CodebookService.Contracts.Endpoints.SmsNotificationTypes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIS.InternalServices.NotificationService.Api.Services.AuditLog;

[ScopedService, SelfService]
public class SmsAuditLogger
{
    // todo: change application logging to audit logging
    private readonly IDateTime _dateTime;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<SmsAuditLogger> _logger;

    public SmsAuditLogger(
        IDateTime dateTime,
        IHttpContextAccessor httpContextAccessor,
        ILogger<SmsAuditLogger> logger)
    {
        _dateTime = dateTime;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    private async Task<string> GetBodyFromRequest(HttpRequest request)
    {
        request.EnableBuffering();
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
        
        var log = new AuditHttpRequestLog
        {
            TraceId = httpContext.TraceIdentifier,
            RequestDateTimeUtc = _dateTime.Now,
            ClientIp = httpContext.Connection.RemoteIpAddress?.ToString(),
            RequestMethod = httpContext.Request.Method,
            RequestPath = httpContext.Request.Path,
            RequestQuery = httpContext.Request.QueryString.ToString(),
            RequestHeaders = httpContext.Request.Headers.ToDictionary(v => v.Key, v => v.Value),
            RequestBody = await GetBodyFromRequest(httpContext.Request),
            RequestScheme = httpContext.Request.Scheme,
            RequestHost = httpContext.Request.Host.ToString(),
            RequestContentType = httpContext.Request.ContentType
        };

        var logMessage = JsonConvert.SerializeObject(log);

        _logger.LogInformation(logMessage);
    }

    public async Task LogHttpResponse(IActionResult? actionResult)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null) return;

        var log = new AuditHttpResponseLog
        {
            ResponseDateTimeUtc = _dateTime.Now,
            TraceId = httpContext.TraceIdentifier,
            ResponseContentType = httpContext.Response.ContentType,
            ResponseStatus = httpContext.Response.StatusCode.ToString(),
            ResponseHeaders = httpContext.Response.Headers.ToDictionary(v => v.Key, v => v.Value),
            ResponseBody = await GetBodyFromResponse(httpContext.Response)
        };
        
        if (actionResult is ObjectResult result)
        {
            log.ResponseStatus = result.StatusCode?.ToString() ?? string.Empty;
            log.ResponseBody = JsonConvert.SerializeObject(result.Value);
        }

        var logMessage = JsonConvert.SerializeObject(log);

        _logger.LogInformation(logMessage);
    }

    public void LogHttpException(Exception exception)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null) return;

        var log = new AuditHttpExceptionLog
        {
            TraceId = httpContext.TraceIdentifier,
            ResponseDateTimeUtc = _dateTime.Now,
            ResponseStatus = httpContext.Response.StatusCode.ToString(),
            Exception = exception
        };

        var logMessage = JsonConvert.SerializeObject(log);

        _logger.LogInformation(logMessage);
    }

    public void LogKafkaProducing(SmsNotificationTypeItem smsType, string consumer)
    {
        if (smsType.IsAuditLogEnabled)
        {
            _logger.LogInformation("Producing message SendSMS with type '{SmsType}' to KAFKA by consumer '{Consumer}'.",
                smsType.Code, consumer);
        }
    }

    public void LogKafkaProduced(SmsNotificationTypeItem smsType, Guid notificationId, string consumer)
    {
        if (smsType.IsAuditLogEnabled)
        {
            _logger.LogInformation(
                "Produced message SendSMS with type '{SmsType}' and notification id '{NotificationId}' to KAFKA by consumer '{Consumer}'.",
                smsType.Code, notificationId, consumer);
        }
    }

    public void LogKafkaError(SmsNotificationTypeItem smsType, string consumer)
    {
        if (smsType.IsAuditLogEnabled)
        {
            _logger.LogInformation(
                "Could not produce message SendSMS with type '{SmsType}' to KAFKA by consumer '{Consumer}'.",
                smsType.Code, consumer);
        }
    }

    public void LogKafkaResultReceived(NotificationReport report)
    {
        _logger.LogInformation("Received notification report {@Report}.", new
        {
            NotificationId = report.id,
            State = report.state,
            Errors = report.notificationErrors
                .Select(e => new
                {
                    Code = e.code,
                    Message = e.message
                })
                .ToList()
        });
    }
}