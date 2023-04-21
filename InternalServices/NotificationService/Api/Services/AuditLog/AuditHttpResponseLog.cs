using Microsoft.Extensions.Primitives;

namespace CIS.InternalServices.NotificationService.Api.Services.AuditLog;

public class AuditHttpResponseLog : AuditLog
{
    public DateTime? ResponseDateTimeUtc { get; set; }
    public string ResponseStatus { get; set; } = null!;
    public Dictionary<string, StringValues> ResponseHeaders { get; set; } = null!;
    public string ResponseBody { get; set; } = null!;
    public string? ResponseContentType { get; set; }
}