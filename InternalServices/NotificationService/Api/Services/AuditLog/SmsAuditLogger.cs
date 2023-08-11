﻿using System.Globalization;
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
    
    public async Task LogHttpRequestProcessed(IActionResult? actionResult)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null) return;

        var httpHeaders = httpContext.Request.Headers
            .ToDictionary(v => v.Key, v => v.Value);
        
        var rawHttpRequestHeaders = JsonConvert.SerializeObject(httpHeaders);
        var rawHttpRequestBody = await GetBodyFromRequest(httpContext.Request);
        
        var rawHttpResponseBody = await GetBodyFromResponse(httpContext.Response);
        
        if (actionResult is ObjectResult result)
        {
            rawHttpResponseBody = JsonConvert.SerializeObject(result.Value);
        }
        
        _auditLogger.LogWithCurrentUser(
            AuditEventTypes.Noby012,
            "HTTP request processed",
            bodyBefore: new Dictionary<string, string>
            {
                { "requestPath", httpContext.Request.Path },
                { "requestQuery", httpContext.Request.QueryString.ToString() },
                { "rawHttpRequestHeaders", ToLiteral(rawHttpRequestHeaders) },
                { "rawHttpRequestBody", ToLiteral(rawHttpRequestBody) }
            },
            bodyAfter: new Dictionary<string, string>
            {
                { "traceId", httpContext.TraceIdentifier },
                { "responseStatus", httpContext.Response.StatusCode.ToString(CultureInfo.InvariantCulture) }, 
                { "rawHttpResponseBody", ToLiteral(rawHttpResponseBody) }
            }
        );
    }

    public async Task LogHttpRequestError(Exception exception)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null) return;
        
        var httpHeaders = httpContext.Request.Headers
            .ToDictionary(v => v.Key, v => v.Value);
        
        var rawHttpRequestHeaders = JsonConvert.SerializeObject(httpHeaders);
        var rawHttpRequestBody = await GetBodyFromRequest(httpContext.Request);
        
        var rawException = JsonConvert.SerializeObject(exception);
        
        _auditLogger.LogWithCurrentUser(
            AuditEventTypes.Noby012,
            "HTTP request error",
            bodyBefore: new Dictionary<string, string>
            {
                { "requestPath", httpContext.Request.Path },
                { "requestQuery", httpContext.Request.QueryString.ToString() },
                { "rawHttpRequestHeaders", ToLiteral(rawHttpRequestHeaders) },
                { "rawHttpRequestBody", ToLiteral(rawHttpRequestBody) }
            },
            bodyAfter: new Dictionary<string, string>
            {
                { "traceId", httpContext.TraceIdentifier },
                { "exception", ToLiteral(rawException) },
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

    public void LogKafkaProduceError(SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, string consumer, string errorMessage)
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
                }
            );
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
                    { "errors", ToLiteral(JsonConvert.SerializeObject(report.notificationErrors)) }
                });
        }
    }
    
    private static string ToLiteral(string input) {
        var literal = new StringBuilder(input.Length );
        foreach (var c in input) {
            switch (c) {
                case '\"': literal.Append("\\\""); break;
                case '\\': literal.Append(@"\\"); break;
                case '\0': literal.Append(@"\0"); break;
                case '\a': literal.Append(@"\a"); break;
                case '\b': literal.Append(@"\b"); break;
                case '\f': literal.Append(@"\f"); break;
                case '\n': literal.Append(@"\n"); break;
                case '\r': literal.Append(@"\r"); break;
                case '\t': literal.Append(@"\t"); break;
                case '\v': literal.Append(@"\v"); break;
                default:
                    // ASCII printable character
                    if (c >= 0x20 && c <= 0x7e) {
                        literal.Append(c);
                        // As UTF16 escaped character
                    } else {
                        literal.Append(@"\u");
                        literal.Append(((int)c).ToString("x4"));
                    }
                    break;
            }
        }
        return literal.ToString();
    }
}