namespace CIS.InternalServices.NotificationService.Api.Endpoints.Infrastructure.AuditLog;

public abstract class AuditLog
{
    public string TraceId { get; set; } = null!;
}