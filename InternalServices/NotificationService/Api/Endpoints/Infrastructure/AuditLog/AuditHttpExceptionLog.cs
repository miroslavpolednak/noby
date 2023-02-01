namespace CIS.InternalServices.NotificationService.Api.Endpoints.Infrastructure.AuditLog;

public class AuditHttpExceptionLog : AuditLog
{
    public DateTime ResponseDateTimeUtc { get; set; }
    public string ResponseStatus { get; set; } = null!;
    public Exception Exception { get; set; } = null!;
}