namespace CIS.InternalServices.NotificationService.Api.Services.AuditLog;

public abstract class AuditLog
{
    public string TraceId { get; set; } = null!;
}