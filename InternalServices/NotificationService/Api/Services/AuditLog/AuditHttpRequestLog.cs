using Microsoft.Extensions.Primitives;

namespace CIS.InternalServices.NotificationService.Api.Services.AuditLog;

public class AuditHttpRequestLog : AuditLog
{
    public DateTime RequestDateTimeUtc { get; set; }
    public string? ClientIp { get; set; }
    public string RequestPath { get; set; } = null!;
    public string RequestQuery { get; set; } = null!;
    public string RequestMethod { get; set; } = null!;
    public string RequestScheme { get; set; } = null!;
    public string RequestHost { get; set; } = null!;
    public Dictionary<string, StringValues> RequestHeaders { get; set; } = null!;
    public string RequestBody { get; set; } = null!;
    public string? RequestContentType { get; set; }
}