using CIS.Infrastructure.Telemetry.AuditLog;

namespace CIS.Infrastructure.Telemetry;

public interface IAuditLogger
{
    void Log(
        AuditEventTypes eventType,
        string message,
        ICollection<AuditLoggerHeaderItem>? identities = null,
        ICollection<AuditLoggerHeaderItem>? products = null,
        AuditLoggerHeaderItem? operation = null,
        string? result = null,
        IDictionary<string, string>? bodyBefore = null,
        IDictionary<string, string>? bodyAfter = null);

    void LogWithCurrentUser(
        AuditEventTypes eventType,
        string message,
        ICollection<AuditLoggerHeaderItem>? identities = null,
        ICollection<AuditLoggerHeaderItem>? products = null,
        AuditLoggerHeaderItem? operation = null,
        string? result = null,
        IDictionary<string, string>? bodyBefore = null,
        IDictionary<string, string>? bodyAfter = null);
}